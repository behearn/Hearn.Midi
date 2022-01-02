using Hearn.Midi.Extensions;
using Hearn.Midi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Hearn.Midi.MidiConstants;
using static Hearn.Midi.MidiEventConstants;

namespace Hearn.Midi
{
    public class MidiStreamWriter : IDisposable
    {

        Stream _stream;
        int _tracks = -1;
        int _lastTrack = 0;
        int _currentTrack = -1;
        long _trackStart;
        bool _streamIsOpen;
        long _currentTick;
        long _awaitedTick;

        List<PlayingNote> _playingNotes;        

        public enum StringTypes
        {
            ArbitraryText = 0x01,
            CopyrightNotice = 0x02,
            TrackName = 0x03,
            InstrumentName = 0x04,
            Lyric = 0x05,
            Marker = 0x06,
            CuePoint = 0x07
        }

        public enum NoteDurations
        {
            HemidemisemiQuaver = 6,
            SixtyFourthNote = 6,

            HemidemisemiQuaverDotted = 9,
            SixtyFourthNoteDotted = 9,

            DemisemiQuaver = 12,
            ThirtySecondNote = 12,

            DemisemiQuaverDotted = 18,
            ThirtySecondNoteDotted = 18,

            SemiQuaver = 24,
            SixteenthNote = 24,

            Triplet = 32,

            SemiQuaverDotted = 36,
            SixteenthNoteDotted = 36,

            Quaver = 48,
            EighthNote = 48,

            QuaverDotted = 72,
            EighthNoteDotted = 73,

            Crotchet = 96,
            QuarterNote = 96,

            CrotchetDotted = 144,
            QuarterNoteDotted = 144,

            Minim = 192,
            HalfNote = 192,

            MinimDotted = 288,
            HalfNoteDotted = 288,

            SemiBreve = 384,
            WholeNote = 384,

            SemiBreveDotted = 576,
            WholeNoteDotted = 576,

            Breve = 768,
            DoubleWholeNote = 768,

            BreveDotted = 1152,
            DoubleWholeNoteDotted = 1152
        }

        /// <summary>
        /// Returns the number of tracks specified in WriteHeader. Returns -1 if no header written
        /// </summary>
        public int Tracks { get => _tracks; }

        /// <summary>
        /// Returns the current track set by WriteStartTrack. Returns -1 if not currently writing a track
        /// </summary>
        public int CurrentTrack { get => _currentTrack; }

        /// <summary>
        /// Creates a new MidiStreamWriter instance
        /// </summary>
        /// <param name="stream">An instance of an object derived from System.IO.Stream</param>
        public MidiStreamWriter(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentException("stream has not been instantiated");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("stream must be writable");
            }

            _stream = stream;
            _streamIsOpen = true;

            _playingNotes = new List<PlayingNote>();
        }

        /// <summary>
        /// Writes the Midi file header.  Must be the first method called
        /// </summary>
        /// <param name="format">Indicates whether this is a single or multi track file</param>
        /// <param name="tracks">Number of tracks</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteHeader(Formats format, byte tracks)
        {

            const long HEADER_LENGTH = 6;

            if (_tracks > 0)
            {
                throw new InvalidOperationException("WriteHeader has already been called");
            }

            if (tracks == 0)
            {
                throw new ArgumentException("tracks cannot be zero");
            }

            if (format == Formats.SingleTrack && tracks != 1)
            {
                throw new ArgumentException("tracks must be 1 when format is SingleTrack");
            }

            var header = Encoding.ASCII.GetBytes("MThd");
            _stream.Write(header, 0, 4);

            _stream.WriteLong(HEADER_LENGTH);
            _stream.WriteInt((int)format);
            _stream.WriteInt(tracks);
            _stream.WriteInt((int)NoteDurations.QuarterNote);

            _tracks = tracks;

            return this;
        }

        /// <summary>
        /// Start a track.  Must be called after WriteHeader, or WriteEndTrack for multi track streams
        /// </summary>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteStartTrack()
        {
            if (_tracks == -1)
            {
                throw new InvalidOperationException($"Create header before starting a new track");
            }

            if (_currentTrack != -1)
            {
                throw new InvalidOperationException($"End track {_currentTrack} before starting a new track");
            }

            _currentTrack = _lastTrack + 1;

            if (_currentTrack > _tracks)
            {
                throw new InvalidOperationException("Max number of tracks reached");
            }

            var header = Encoding.ASCII.GetBytes("MTrk");
            _stream.Write(header, 0, 4);

            _trackStart = _stream.Position;

            _stream.WriteLong(0); //Placeholder for length

            _currentTick = 0;
            _awaitedTick = 0;

            return this;
        }

        /// <summary>
        /// Ends the current track
        /// </summary>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteEndTrack()
        {
            if (_currentTrack == -1)
            {
                throw new ArgumentException($"Start track must be called first");
            }

            if (_playingNotes.Any())
            {
                _awaitedTick = _playingNotes.Max(x => x.EndTick);
                StopPlayedNotes();
            }

            //Overwrite length placeholder with actual length
            var trackLength = _stream.Position - _trackStart;
            _stream.Seek(_trackStart, SeekOrigin.Begin);
            _stream.WriteLong(trackLength);

            _stream.Seek(0, SeekOrigin.End);

            //End of track = 00 FF 2F 00
            _stream.WriteByte(0x00); //Delta time
            _stream.WriteByte(META_EVENT);
            _stream.WriteInt(META_EVENT_END_OF_TRACK);

            _lastTrack = _currentTrack;
            _currentTrack = -1;
            _trackStart = -1;

            return this;
        }

        /// <summary>
        /// Writes a string of text to a track
        /// </summary>
        /// <param name="stringType">Type of text meta event</param>
        /// <param name="text">Text to write</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteString(StringTypes stringType, string text)
        {
            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteString must be called after WriteStartTrack");
            }

            if (text.Length > 127)
            {
                throw new ArgumentException("text exceeds 127 characters");
            }

            //Text Event = 00 FF 0x<stringType> 
            _stream.WriteByte(0x00);
            _stream.WriteByte(META_EVENT);
            _stream.WriteByte((byte)stringType);

            _stream.WriteByte((byte)text.Length);
            var bytes = Encoding.ASCII.GetBytes(text);
            _stream.Write(bytes, 0, bytes.Length);

            return this;
        }

        /// <summary>
        /// Changes the tempo for the track (if not supplied MIDI will default to 120bpm)
        /// </summary>
        /// <param name="bpm">Beats per minute</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteTempo(int bpm)
        {
            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteTempo must be called after WriteStartTrack");
            }

            if (bpm < 0)
            {
                throw new ArgumentException("Tempo cannot be negative");
            }
            else if (bpm == 0)
            {
                throw new ArgumentException("Tempo cannot be zero");
            }
           
            const long MICROSECONDS_PER_MINUTE = 60000000;

            var tempo = MICROSECONDS_PER_MINUTE / bpm;

            _stream.WriteByte(0x00); //Delta time

            //Tempo = FF 51 03
            _stream.WriteByte(META_EVENT);
            _stream.WriteInt(META_EVENT_SET_TEMPO);

            _stream.Write24bitInt(tempo);

            return this;

        }

        /// <summary>
        /// Writes the time signature (if not supplied MIDI will default to 4/4 time)
        /// </summary>
        /// <param name="topNumber">Beats per bar</param>
        /// <param name="bottomNumber">Type of notes (MIDI spec dictates this must be a power of 2)</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteTimeSignature(byte topNumber, byte bottomNumber)
        {
            const byte MIDI_CLOCKS_PER_TICK = 24;
            const byte NO_32ND_NOTES_PER_24_MIDI_CLOCKS = 8;

            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteTimeSignature must be called after WriteStartTrack");
            }

            if (topNumber == 0)
            {
                throw new ArgumentException("topNumber cannot be zero");
            }

            _stream.WriteByte(0x00); //Delta time

            //Time signature = FF 58 
            _stream.WriteByte(META_EVENT);
            _stream.WriteInt(META_EVENT_TIME_SIGNATURE);

            _stream.WriteByte(topNumber);

            switch (bottomNumber)
            {
                case 2:
                    _stream.WriteByte(1);
                    break;
                case 4:
                    _stream.WriteByte(2);
                    break;
                case 8:
                    _stream.WriteByte(3);
                    break;
                case 16:
                    _stream.WriteByte(4);
                    break;
                default:
                    throw new ArgumentException("Invalid bottomNumber (valid values are 2, 4, 8 & 16)");
            }

            _stream.WriteByte(MIDI_CLOCKS_PER_TICK);
            _stream.WriteByte(NO_32ND_NOTES_PER_24_MIDI_CLOCKS);

            return this;

        }

        /// <summary>
        /// Changes the instrument for the supplied channel
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="instrument">instrument</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteChangeInstrument(byte channel, Instruments instrument)
        {

            const byte PROGRAM_CHANGE_EVENT = 0xC0;

            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteChangeInstrument must be called after WriteStartTrack");
            }

            if (channel < 0 || channel > 15)
            {
                throw new ArgumentException("channel must be in the range 0..15");
            }

            var eventCode = (byte)(PROGRAM_CHANGE_EVENT | channel);

            _stream.WriteByte(0x00); //Delta time
            _stream.WriteByte(eventCode);
            _stream.WriteByte((byte)instrument);

            return this;

        }

        /// <summary>
        /// Writes a note to a channel but does not tick over the current time.  Use Tick(length) to control the start time of further notes
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="midiNote">An instance of the MidiNote class</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNote(byte channel, MidiNote midiNote)
        {
            return WriteNote(channel, midiNote.Note, midiNote.Velocity, midiNote.Duration);
        }

        /// <summary>
        /// Writes a note to a channel and ticks over the current time, stopping any playing notes due to end
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="note">note to play</param>
        /// <param name="velocity">soft to loud [0..127]</param>
        /// <param name="length">duration of the note</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNoteAndTick(byte channel, MidiNoteNumbers note, byte velocity, NoteDurations length)
        {
            WriteNote(channel, note, velocity, (long)length);
            Tick(length);
            return this;
        }

        /// <summary>
        /// Writes a note to a channel and ticks over the current time, stopping any playing notes due to end
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="note">note to play</param>
        /// <param name="velocity">soft to loud [0..127]</param>
        /// <param name="length">duration of the note (see NoteDuration enum values)</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNoteAndTick(byte channel, MidiNoteNumbers note, byte velocity, long length)
        {
            WriteNote(channel, note, velocity, length);
            Tick(length);
            return this;
        }

        /// <summary>
        /// Writes a note to a channel but does not tick over the current time.  Use Tick(length) to control the start time of further notes
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="note">note to play</param>
        /// <param name="velocity">soft to loud [0..127]</param>
        /// <param name="length">duration of the note</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNote(byte channel, MidiNoteNumbers note, byte velocity, NoteDurations length)
        {
            return WriteNote(channel, note, velocity, (long)length);
        }

        /// <summary>
        /// Writes a note to a channel but does not tick over the current time.  Use Tick(length) to control the start time of further notes
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="note"></param>
        /// <param name="velocity">soft to loud [0..127]</param>
        /// <param name="length">duration of the note (see NoteDuration enum values)</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNote(byte channel, MidiNoteNumbers note, byte velocity, long length)
        {

            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteNote must be called after WriteStartTrack");
            }

            if (channel < 0 || channel > 15)
            {
                throw new ArgumentException("channel must be in the range 0..15");
            }

            if (velocity < 0 || velocity > 127)
            {
                throw new ArgumentException("velocity must be in the range 0..127");
            }

            var deltaTime = CalculateDelta();

            var eventCode = (byte)(NOTE_ON_EVENT | channel);

            _stream.WriteVariableLengthQuantity(deltaTime);
            _stream.WriteByte(eventCode);
            _stream.WriteByte((byte)note);
            _stream.WriteByte(velocity);


            _playingNotes.Add(new PlayingNote()
            {
                Channel = channel,
                Note = note,
                StartTick = _currentTick,
                EndTick = _currentTick + (long)length
            });

            return this;

        }

        /// <summary>
        /// Writes a group of notes to a channel but does not tick over the current time.  Use Tick(length) to control the start time of further notes
        /// </summary>
        /// <param name="channel">channel number [0..15] (Note: channel 10 ([9]) is reserved for percussion)</param>
        /// <param name="midiNotes">List of MidiNote instances to play</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter WriteNotes(byte channel, IEnumerable<MidiNote> midiNotes)
        {
            if (_currentTrack == -1)
            {
                throw new InvalidOperationException("WriteNotes must be called after WriteStartTrack");
            }

            if (channel < 0 || channel > 15)
            {
                throw new ArgumentException("channel must be in the range 0..15");
            }

            if (midiNotes == null || midiNotes.ToArray().Length == 0)
            {
                throw new ArgumentException("midiNotes not supplied");
            }

            StopPlayedNotes();

            var deltaTime = CalculateDelta();

            var eventCode = (byte)(NOTE_ON_EVENT | channel);

            _stream.WriteVariableLengthQuantity(deltaTime);
            _stream.WriteByte(eventCode);

            var i = 0;
            foreach (var midiNote in midiNotes)
            {

                if (midiNote.Velocity < 0 || midiNote.Velocity > 127)
                {
                    throw new ArgumentException("velocity must be in the range 0..127");
                }

                if (i > 0)
                {
                    _stream.WriteVariableLengthQuantity(0x00); //Delta time
                }

                _stream.WriteByte((byte)midiNote.Note);
                _stream.WriteByte(midiNote.Velocity);

                _playingNotes.Add(new PlayingNote()
                {
                    Channel = channel,
                    Note = midiNote.Note,
                    StartTick = _currentTick,
                    EndTick = _currentTick + (long)midiNote.Duration
                });

                i++;

            }

            return this;

        }

        /// <summary>
        /// Writes percussion beat to channel 10 ([9]) 
        /// </summary>
        /// <param name="percussion"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public MidiStreamWriter WritePercussion(Percussion percussion, byte velocity)
        {
            const byte PERCUSSION_CHANNEL = 9;
            WriteNote(PERCUSSION_CHANNEL, (MidiNoteNumbers)percussion, velocity, NoteDurations.QuarterNote);
            return this;
        }

        /// <summary>
        /// Updates the current time of the track by a NoteDuration, stopping any playing notes due to end
        /// </summary>
        /// <param name="duration">length of time to tick over</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter Tick(NoteDurations duration)
        {
            return Tick((long)duration);
        }

        /// <summary>
        /// Updates the current time of the track by a NoteDuration, stopping any playing notes due to end
        /// </summary>
        /// <param name="duration">length of time to tick over (See NoteDurations enum values)</param>
        /// <returns>Current MidiStreamWriter instance</returns>
        public MidiStreamWriter Tick(long duration)
        {
            _awaitedTick = _currentTick + duration;
            StopPlayedNotes();

            return this;
        }

        /// <summary>
        /// Writes the buffer to the underlying stream
        /// </summary>
        public void Flush()
        {            
            _stream.Flush();
        }

        /// <summary>
        /// Close the stream
        /// </summary>
        public void Close()
        {
            if (_streamIsOpen)
            {
                _stream.Flush();
                _streamIsOpen = false;
                _stream.Close();
            }            
        }

        private void StopPlayedNotes()
        {

            var notesToStop = _playingNotes.Where(x => x.EndTick <= _awaitedTick);

            if (notesToStop.Any())
            {
                var lastChannel = 0;
                var i = 0;
                foreach (var note in notesToStop.OrderBy(x => x.EndTick).ThenBy(x => x.Id))
                {
                    var deltaTime = note.EndTick - _currentTick;
                    _currentTick = note.EndTick;
                    
                    _stream.WriteVariableLengthQuantity(deltaTime);

                    if (i == 0 || lastChannel != note.Channel)
                    {                        
                        var eventCode = (byte)(NOTE_OFF_EVENT | note.Channel);                        
                        _stream.WriteByte(eventCode);
                        lastChannel = note.Channel;                        
                    }

                    _stream.WriteByte((byte)note.Note);
                    _stream.WriteByte(0x00); //Velocity

                    i++;
                }

                _playingNotes.RemoveAll(x => x.EndTick <= _awaitedTick);
            }
        }

        private long CalculateDelta()
        {
            var deltaTime = _awaitedTick - _currentTick;
            if (deltaTime < 0)
            {
                deltaTime = 0;
                _awaitedTick = _currentTick;
            }
            _currentTick += deltaTime;
            return deltaTime;
        }

        public void Dispose()
        {
            if (_currentTrack != -1)
            {
                throw new ArgumentException($"MidiStreamWriter disposed while writing track {_currentTrack}");
            }

            if (_lastTrack != _tracks)
            {
                throw new ArgumentException($"MidiStreamWriter disposed before writing all {_tracks} tracks");
            }

            if (_streamIsOpen)
            {
                _stream.Flush();
                _stream.Close();
            }

        }
    }
}
