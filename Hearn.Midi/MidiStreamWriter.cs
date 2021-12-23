using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Hearn.Midi.MidiConstants;

namespace Hearn.Midi
{
    public class MidiStreamWriter : IDisposable
    {

        const byte META_EVENT = 0xFF;
        const int META_EVENT_END_OF_TRACK = 0x2F00;
        const int META_EVENT_SET_TEMPO = 0x5103;
        const int META_EVENT_TIME_SIGNATURE = 0x5804;

        const byte NOTE_ON_EVENT = 0x90;
        const byte NOTE_OFF_EVENT = 0x80;

        Stream _stream;
        int _tracks;
        int _lastTrack = 0;
        int _currentTrack = -1;
        long _trackStart;
        bool _streamIsOpen;
        long _currentTick;
        long _awaitedTick;

        List<PlayingNote> _playingNotes;

        public enum Formats
        {
            SingleTrack = 0,
            MultiSimultaneousTracks = 1,
            MultiSequentialTracks = 2
        }

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

        public MidiStreamWriter(Stream stream)
        {
            _stream = stream;
            _streamIsOpen = true;

            _playingNotes = new List<PlayingNote>();
        }

        public MidiStreamWriter WriteHeader(Formats format, byte tracks)
        {

            const long HEADER_LENGTH = 6;

            if (tracks < 0)
            {
                throw new ArgumentException("tracks cannot be less than zero");
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
        /// Start a track
        /// </summary>
        /// <returns>Current MidiStreamWriter for fluent API</returns>
        public MidiStreamWriter WriteStartTrack()
        {
            if (_currentTrack != -1)
            {
                throw new ArgumentException($"End track {_currentTrack} before starting a new track");
            }

            _currentTrack = _lastTrack + 1;

            if (_currentTrack > _tracks)
            {
                throw new ArgumentException("Max number of tracks reached");
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
        /// <returns>Current MidiStreamWriter for fluent API</returns>
        public MidiStreamWriter WriteEndTrack()
        {
            if (_currentTrack == -1)
            {
                throw new ArgumentException($"Start track must be called first");
            }

            if (_playingNotes.Any())
            {
                _awaitedTick = _playingNotes.Max(x => x.EndTick);
                UpdateCurrentTime();
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
        /// Writes a string of text to a text meta event
        /// </summary>
        /// <param name="stringType">Type of text meta event</param>
        /// <param name="text">Text to write</param>
        /// <returns>Current MidiStreamWriter for fluent API</returns>
        public MidiStreamWriter WriteString(StringTypes stringType, string text)
        {
            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteString must be called after WriteStartTrack");
            }

            if (text.Length > 127)
            {
                throw new ArgumentException("text exceeds 127 characters");
            }

            //Text Event = 00 FF 01 
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
        /// <returns>Current MidiStreamWriter for fluent API</returns>
        public MidiStreamWriter WriteTempo(int bpm)
        {

            if (bpm <= 0)
            {
                throw new ArgumentException("Tempo cannot be negative");
            }
            else if (bpm == 0)
            {
                throw new ArgumentException("Tempo cannot be zero");
            }

            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteTempo must be called after WriteStartTrack");
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
        /// Writes the time signature
        /// </summary>
        /// <param name="topNumber">Beats per bar</param>
        /// <param name="bottomNumber">Type of notes (MIDI spec dictates this must be a power of 2)</param>
        /// <returns>Current MidiStreamWriter for fluent API</returns>
        public MidiStreamWriter WriteTimeSignature(byte topNumber, byte bottomNumber)
        {
            const byte MIDI_CLOCKS_PER_TICK = 24;
            const byte NO_32ND_NOTES_PER_24_MIDI_CLOCKS = 8;

            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteTimeSignature must be called after WriteStartTrack");
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

        public MidiStreamWriter WriteChangeInstrument(byte channel, Instruments instrument)
        {

            const byte PROGRAM_CHANGE_EVENT = 0xC0;

            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteChangeInstrument must be called after WriteStartTrack");
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

        public MidiStreamWriter WriteNote(byte channel, MidiNote midiNote)
        {
            return WriteNote(channel, midiNote.Note, midiNote.Velocity, midiNote.Duration);
        }

        public MidiStreamWriter WriteNoteAndWait(byte channel, MidiNoteNumbers note, byte velocity, NoteDurations length)
        {
            WriteNote(channel, note, velocity, (long)length);
            Wait(length);
            return this;
        }

        public MidiStreamWriter WriteNoteAndWait(byte channel, MidiNoteNumbers note, byte velocity, long length)
        {
            WriteNote(channel, note, velocity, length);
            Wait(length);
            return this;
        }

        public MidiStreamWriter WriteNote(byte channel, MidiNoteNumbers note, byte velocity, NoteDurations length)
        {
            return WriteNote(channel, note, velocity, (long)length);
        }

        public MidiStreamWriter WriteNote(byte channel, MidiNoteNumbers note, byte velocity, long length)
        {

            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteNote must be called after WriteStartTrack");
            }

            if (channel < 0 || channel > 15)
            {
                throw new ArgumentException("channel must be in the range 0..15");
            }

            if (velocity < 0 || velocity > 127)
            {
                throw new ArgumentException("velocity must be in the range 0..127");
            }

            UpdateCurrentTime();

            var deltaTime = UpdateDelta();

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

        public MidiStreamWriter WriteNotes(byte channel, IEnumerable<MidiNote> midiNotes)
        {
            if (_currentTrack == -1)
            {
                throw new ArgumentException("WriteNotes must be called after WriteStartTrack");
            }

            if (channel < 0 || channel > 15)
            {
                throw new ArgumentException("channel must be in the range 0..15");
            }

            if (midiNotes == null || midiNotes.ToArray().Length == 0)
            {
                throw new ArgumentException("midiNotes not supplied");
            }

            UpdateCurrentTime();

            var deltaTime = UpdateDelta();

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
                    _stream.WriteVariableLengthQuantity(deltaTime);
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

        public MidiStreamWriter Wait(NoteDurations duration)
        {
            return Wait((long)duration);
        }

        public MidiStreamWriter Wait(long duration)
        {
            _awaitedTick = _currentTick + duration;
            UpdateCurrentTime();

            return this;
        }

        public void Flush()
        {
            _stream.Flush();
        }

        public void Close()
        {
            _streamIsOpen = false;
            _stream.Close();
        }

        private void UpdateCurrentTime()
        {

            var notesToStop = _playingNotes.Where(x => x.EndTick <= _awaitedTick);

            if (notesToStop.Any())
            {
                var lastChannel = 0;
                var i = 0;
                foreach (var note in notesToStop.OrderBy(x => x.EndTick).ThenBy(x => x.Id))
                {
                    if (i == 0 || lastChannel != note.Channel)
                    {
                        var deltaTime = note.EndTick - _currentTick;

                        var eventCode = (byte)(NOTE_OFF_EVENT | note.Channel);
                        _stream.WriteVariableLengthQuantity(deltaTime);
                        _stream.WriteByte(eventCode);
                        lastChannel = note.Channel;

                        _currentTick = note.EndTick;
                    }
                    else if (i > 0)
                    {
                        _stream.WriteVariableLengthQuantity(0x00); //Delta time
                    }

                    _stream.WriteByte((byte)note.Note);
                    _stream.WriteByte(0x00); //Velocity

                    i++;
                }

                _playingNotes.RemoveAll(x => x.EndTick <= _awaitedTick);
            }
        }

        private long UpdateDelta()
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
