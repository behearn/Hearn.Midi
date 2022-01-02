using Hearn.Midi.MidiData;
using Hearn.Midi.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi
{
    public class MidiStreamReader
    {

        Stream _stream;

        bool _headerRead;
        bool _inTrack;
        int _trackNo;
        byte _runningStatus;

        public MidiStreamReader(Stream stream)
        {
            _stream = stream;
        }

        public BaseMidiData Read()
        {
            if (!_headerRead)
            {
                return ReadHeaderChunk();
            }
            else if (!_inTrack)
            {
                return ReadTrackChunk();
            }
            else
            {
                return ReadEvent();
            }
        }

        private HeaderChunk ReadHeaderChunk()
        {

            var headerChunk = new HeaderChunk();

            var headerBytes = new byte[4];
            _stream.Read(headerBytes, 0, 4);
            var header = Encoding.ASCII.GetString(headerBytes);
            if (header != "MThd")
            {
                throw new InvalidDataException("MThd header not found");
            }

            var headerLength = _stream.ReadLong();
            if (headerLength != 6)
            {
                throw new InvalidDataException($"Invalid MThd length (Expected 6 Actual {headerLength})");
            }

            var formatInt = _stream.ReadInt();
            switch(formatInt)
            {
                case 0:
                case 1:
                case 2:
                    headerChunk.Format = (MidiConstants.Formats)formatInt;
                    break;
                default:
                    throw new InvalidDataException("Invalid MThd format");
            }

            headerChunk.Tracks = _stream.ReadInt();

            headerChunk.Division = _stream.ReadInt();

            _headerRead = true;

            return headerChunk;
        }

        private TrackChunk ReadTrackChunk()
        {

            var headerBytes = new byte[4];
            _stream.Read(headerBytes, 0, 4);
            var header = Encoding.ASCII.GetString(headerBytes);
            if (header != "MTrk")
            {
                throw new InvalidDataException("MTrk not found");
            }

            _trackNo++;

            var pos = _stream.Position;

            var trackLength = _stream.ReadLong();
            if (trackLength < 0)
            {
                throw new InvalidDataException($"MTrk ({_trackNo}) track length is negative");
            }
            if (pos + trackLength > _stream.Length)
            {
                throw new InvalidDataException($"MTrk track length extends beyond stream length");
            }

            var endOfTrackBytes = new byte[3];
            _stream.Seek(trackLength - 3, SeekOrigin.Current); //-3 
            var metaEvent = (byte)_stream.ReadByte();
            var metaEventType = _stream.ReadInt();
            if (metaEvent != MidiEventConstants.META_EVENT && metaEventType != MidiEventConstants.META_EVENT_END_OF_TRACK)
            {
                throw new InvalidDataException($"MTrk does not contain end of track meta event at expected position");
            }

            _stream.Seek(pos + 4, SeekOrigin.Begin); //+4 to account for ReadLong

            _inTrack = true;

            var trackChunk = new TrackChunk();
            trackChunk.TrackNo = _trackNo;
            trackChunk.Length = trackLength;
            return trackChunk;

        }

        private BaseMidiData ReadEvent()
        {

            var deltaTime = _stream.ReadVariableLengthQuantity();

            //Cast straight to byte as we've validated the stream length already
            var eventCode = (byte)_stream.ReadByte(); 
            
            switch (eventCode)
            {
                case MidiEventConstants.META_EVENT:
                    return ReadMetaEvent(deltaTime);

                default:
                    return ReadMidiEvent(deltaTime, eventCode);
            }

        }

        private MetaEvent ReadMetaEvent(long deltaTime)
        {

            var metaEventType = _stream.ReadInt();

            switch(metaEventType)
            {
                case MidiEventConstants.META_EVENT_TIME_SIGNATURE:
                    return ReadTimeSignatureEvent(deltaTime);

                case MidiEventConstants.META_EVENT_SET_TEMPO:
                    return ReadTempoEvent(deltaTime);

                case MidiEventConstants.META_EVENT_END_OF_TRACK:
                    return ReadEndTrack(deltaTime);

                default:
                    throw new NotImplementedException($"Meta Event {metaEventType} not implemented");
            }

        }

        private TimeSignatureEvent ReadTimeSignatureEvent(long deltaTime)
        {

            var bytes = new byte[4];
            _stream.Read(bytes, 0, bytes.Length);

            var timeSignatureEvent = new TimeSignatureEvent(deltaTime);

            timeSignatureEvent.TopNumber = bytes[0];
            timeSignatureEvent.BottomNumber = (byte)Math.Pow(bytes[1], 2);
            timeSignatureEvent.MidiClocksPerMetronome = bytes[2];
            timeSignatureEvent.ThirtySecondNotesPerClock = bytes[3];

            return timeSignatureEvent;

        }

        private TempoEvent ReadTempoEvent(long deltaTime)
        {

            var microSecondsPerQuarterNote = _stream.Read24bitInt();

            var tempoEvent = new TempoEvent(deltaTime);

            tempoEvent.MicroSecondsPerQuarterNote = microSecondsPerQuarterNote;

            return tempoEvent;

        }

        private EndTrackEvent ReadEndTrack(long deltaTime)
        {

            _inTrack = false;
            _runningStatus = 0x00;

            var endTrackEvent = new EndTrackEvent(deltaTime);
           
            return endTrackEvent;

        }

        private MidiEvent ReadMidiEvent(long deltaTime, byte midiEventCode)
        {
            var midiEventType = (byte)(midiEventCode & MidiEventConstants.MASK_MIDI_EVENT_TYPE);
            var channel = (byte)(midiEventCode & MidiEventConstants.MASK_MIDI_EVENT_CHANNEL);

            MidiEvent midiEvent;

            switch (midiEventType)
            {
                case MidiEventConstants.MIDI_EVENT_PROGRAM_CHANGE:
                    midiEvent = ReadProgramChangeEvent(deltaTime, channel);
                    break;

                case MidiEventConstants.MIDI_EVENT_NOTE_ON:
                    midiEvent = ReadNoteOnEvent(deltaTime, channel);
                    break;

                case MidiEventConstants.MIDI_EVENT_NOTE_OFF:
                    midiEvent = ReadNoteOffEvent(deltaTime, channel);
                    break;

                default:
                    _stream.Seek(-1, SeekOrigin.Current); //Step back so we can re-read what was mis-read as midiEventCode
                    midiEvent = ReadMidiEvent(deltaTime, _runningStatus);
                    break;
            }

            _runningStatus = midiEventCode;

            return midiEvent;
        }

        private ProgramChangeEvent ReadProgramChangeEvent(long deltaTime, byte channel)
        {

            var intstrument = (byte)_stream.ReadByte();

            var programChangeEvent = new ProgramChangeEvent(deltaTime);
            
            programChangeEvent.Channel = channel;
            programChangeEvent.Instrument = (MidiConstants.Instruments)(intstrument);

            return programChangeEvent;

        }

        private NoteOnEvent ReadNoteOnEvent(long deltaTime, byte channel)
        {

            var note = (byte)_stream.ReadByte();
            var velocity = (byte)_stream.ReadByte();

            var noteOnEvent = new NoteOnEvent(deltaTime);

            noteOnEvent.Channel = channel;
            noteOnEvent.Note = (MidiConstants.MidiNoteNumbers)(note);
            noteOnEvent.Velocity = velocity;

            return noteOnEvent;

        }

        private NoteOffEvent ReadNoteOffEvent(long deltaTime, byte channel)
        {

            var note = (byte)_stream.ReadByte();
            var velocity = (byte)_stream.ReadByte();

            var noteOffEvent = new NoteOffEvent(deltaTime);

            noteOffEvent.Channel = channel;
            noteOffEvent.Note = (MidiConstants.MidiNoteNumbers)(note);
            noteOffEvent.Velocity = velocity;

            return noteOffEvent;

        }

    }
}
