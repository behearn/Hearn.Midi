using Hearn.Midi.MidiData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hearn.Midi.MidiConstants;
using static Hearn.Midi.MidiData.BaseMidiData;

namespace Hearn.Midi.Tests.MidiStreamReaderTests
{
    [TestClass]
    public class FormatSpecification0Tests
    {

        MemoryStream _stream;
        MidiStreamReader _midiStreamReader;

        [TestInitialize]
        public void TestInitialise()
        {
            CreateStream();

            _midiStreamReader = new MidiStreamReader(_stream);
        }

        private void CreateStream()
        {

            var bytes = new byte[81]
            {
                0x4D, 0x54, 0x68, 0x64, //MThd             

                0x00 , 0x00 , 0x00 , 0x06, //chunk length 
                0x00 , 0x00, //format 0 
                0x00 , 0x01, //one track 
                0x00 , 0x60, //96 per quarter-note 

                0x4D , 0x54 , 0x72 , 0x6B, //MTrk 
                0x00 , 0x00 , 0x00 , 0x3B ,//chunk length (59)

                0x00 , 0xFF , 0x58 , 0x04 , 0x04 , 0x02 , 0x18 , 0x08, //time signature
                0x00 , 0xFF , 0x51 , 0x03 , 0x07 , 0xA1 , 0x20, //tempo 
                0x00 , 0xC0 , 0x05,
                0x00 , 0xC1 , 0x2E,
                0x00 , 0xC2 , 0x46,
                0x00 , 0x92 , 0x30 , 0x60,
                0x00 , 0x3C , 0x60, //running status
                0x60 , 0x91 , 0x43 , 0x40,
                0x60 , 0x90 , 0x4C , 0x20,
                0x81 , 0x40 , 0x82 , 0x30 , 0x40, //two-byte delta-time
                0x00 , 0x3C , 0x40, //running status
                0x00 , 0x81 , 0x43 , 0x40,
                0x00 , 0x80 , 0x4C , 0x40,
                0x00 , 0xFF , 0x2F , 0x00 //end of track
            };

            _stream = new MemoryStream();
            _stream.Write(bytes, 0, 81);
            
            _stream.Position = 0;

        }

        private void RepeatRead(int noOfReads)
        {
            for (var i = 0; i < noOfReads; i++)
            {
                _midiStreamReader.Read();
            }
        }

        [TestMethod]
        public void MidiStreamReader_FormatSpecification0_ReadHeader()
        {

            //Arrage

            //Act

            var data = _midiStreamReader.Read() as HeaderChunk;

            //Assert

            Assert.IsInstanceOfType(data, typeof(HeaderChunk));
            Assert.AreEqual(MidiDataTypes.HeaderChunk, data.MidiDataType);
            Assert.AreEqual(Formats.SingleTrack, data.Format);
            Assert.AreEqual(1, data.Tracks);
            Assert.AreEqual(96, data.Division);

        }

        [TestMethod]
        public void MidiStreamReader_FormatSpecification0_ReadStartTrack()
        {

            //Arrage

            _midiStreamReader.Read(); //Read header

            //Act

            var data = _midiStreamReader.Read() as TrackChunk;

            //Assert

            Assert.IsInstanceOfType(data, typeof(TrackChunk));
            Assert.AreEqual(MidiDataTypes.TrackChunk, data.MidiDataType);
            Assert.AreEqual(1, data.TrackNo);
            Assert.AreEqual(59, data.Length);

        }

        [TestMethod]
        public void MidiStreamReader_FormatSpecification0_ReadTimeSignature()
        {

            //Arrage

            RepeatRead(2);

            //Act

            var data = _midiStreamReader.Read() as TimeSignatureEvent;

            //Assert

            Assert.IsInstanceOfType(data, typeof(TimeSignatureEvent));
            Assert.AreEqual(MidiDataTypes.MetaEvent, data.MidiDataType);
            Assert.AreEqual(4, data.TopNumber);
            Assert.AreEqual(4, data.BottomNumber);
            Assert.AreEqual(24, data.MidiClocksPerMetronome);
            Assert.AreEqual(8, data.ThirtySecondNotesPerClock);

        }

        [TestMethod]
        public void MidiStreamReader_FormatSpecification0_ReadTempo()
        {

            //Arrage

            RepeatRead(3);

            //Act

            var data = _midiStreamReader.Read() as TempoEvent;

            //Assert

            Assert.IsInstanceOfType(data, typeof(TempoEvent));
            Assert.AreEqual(MidiDataTypes.MetaEvent, data.MidiDataType);
            Assert.AreEqual(120, data.Tempo);

        }

        [TestMethod]
        public void MidiStreamReader_FormatSpecification0_ProgramChange()
        {

            //Arrage

            RepeatRead(4);

            //Act

            var data = new ProgramChangeEvent[3];
            data[0] = _midiStreamReader.Read() as ProgramChangeEvent;
            data[1] = _midiStreamReader.Read() as ProgramChangeEvent;
            data[2] = _midiStreamReader.Read() as ProgramChangeEvent;

            //Assert

            Assert.IsInstanceOfType(data[0], typeof(ProgramChangeEvent));
            Assert.AreEqual(MidiDataTypes.MidiEvent, data[0].MidiDataType);
            Assert.AreEqual(0, data[0].Channel);
            Assert.AreEqual(MidiConstants.Instruments.ElectricPiano2, data[0].Instrument);

            Assert.IsInstanceOfType(data[1], typeof(ProgramChangeEvent));
            Assert.AreEqual(MidiDataTypes.MidiEvent, data[0].MidiDataType);
            Assert.AreEqual(1, data[1].Channel);
            Assert.AreEqual(MidiConstants.Instruments.OrchestralHarp, data[1].Instrument);

            Assert.IsInstanceOfType(data[2], typeof(ProgramChangeEvent));
            Assert.AreEqual(MidiDataTypes.MidiEvent, data[2].MidiDataType);
            Assert.AreEqual(2, data[2].Channel);
            Assert.AreEqual(MidiConstants.Instruments.Bassoon, data[2].Instrument);

        }
    }
}
