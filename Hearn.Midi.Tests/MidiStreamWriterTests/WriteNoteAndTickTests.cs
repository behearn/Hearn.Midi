using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hearn.Midi.MidiConstants;
using static Hearn.Midi.MidiStreamWriter;

namespace Hearn.Midi.Tests.MidiStreamWriterTests
{
    [TestClass]
    public class WriteNoteAndTickTests
    {
        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        [TestInitialize]
        public void TestInitalise()
        {
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);

            _midiStreamWriter.WriteHeader(Formats.MultiSimultaneousTracks, 2);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNoteAndTick_OnAndOff()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNoteAndTick(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            _stream.Seek(-8, SeekOrigin.End);

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x90, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity
        }

        [TestMethod]
        public void MidiStreamWriter_WriteNoteAndTick_LengthOverloadSetsDelta()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNoteAndTick(0, MidiNoteNumbers.C4, 127, 100);

            //Assert           

            _stream.Seek(-8, SeekOrigin.End);

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x90, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x64, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity
        }

        [TestMethod]
        public void MidiStreamWriter_WriteNoteAndTick_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteNoteAndTick(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNoteAndTick_OverloadReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteNoteAndTick(0, MidiNoteNumbers.C4, 127, 100);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }
    }
}
