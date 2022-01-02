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
                return ReadChunk();
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

        private BaseMidiData ReadChunk()
        {

            var deltaTime = _stream.ReadVariableLengthQuantity();

            //Cast straight to byte as we've validated the stream length already
            var midiEventType = (byte)_stream.ReadByte(); 
            
            switch (midiEventType)
            {
                case MidiEventConstants.META_EVENT:
                    return ReadMetaEventChunk();

                default:
                    return ReadMidiEvent(midiEventType);
            }

        }

        private MetaEvent ReadMetaEventChunk()
        {

            var metaEventType = _stream.ReadInt();

            switch(metaEventType)
            {
                case MidiEventConstants.META_EVENT_TIME_SIGNATURE:
                    return ReadTimeSignatureChunk();

                default:
                    throw new NotImplementedException($"Meta Event {metaEventType} not implemented");
            }

        }

        private TimeSignatureEvent ReadTimeSignatureChunk()
        {

            var bytes = new byte[4];
            _stream.Read(bytes, 0, bytes.Length);

            var timeSignatureChunk = new TimeSignatureEvent();

            timeSignatureChunk.TopNumber = bytes[0];
            timeSignatureChunk.BottomNumber = (byte)Math.Pow(bytes[1], 2);
            timeSignatureChunk.MidiClocksPerMetronome = bytes[2];
            timeSignatureChunk.ThirtySecondNotesPerClock = bytes[3];

            return timeSignatureChunk;

        }

        private BaseMidiData ReadMidiEvent(byte midiEventType)
        {
            throw new NotImplementedException();
        }

    }
}
