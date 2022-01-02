using Hearn.Midi.Models;
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
    public class WriteNoteTests
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
        public void MidiStreamWriter_WriteNote_Channel1MiddleC()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x90, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_Channel16MiddleC()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNote(15, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x9F, bytes[1]); //Note on channel 16
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_LengthOverload()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNote(15, MidiNoteNumbers.C4, 127, 100); //See Tick length overload test for better use case

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x9F, bytes[1]); //Note on channel 16
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_MidiNoteOverload()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNote(15, new MidiNote()
            {
                Note = MidiNoteNumbers.C4,
                Velocity = 127,
                Duration = NoteDurations.QuarterNote
            });

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x9F, bytes[1]); //Note on channel 16
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_TickSetsDelta()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .Tick(NoteDurations.HalfNoteDotted); //This is large enough to trigger a variable length quantity

            var bytes = new byte[5];

            //Act

            _midiStreamWriter.WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            _stream.Seek(-5, SeekOrigin.End);
            _stream.Read(bytes, 0, 5);

            Assert.AreEqual(0x82, bytes[0]); //Delta (variable length quantity)
            Assert.AreEqual(0x20, bytes[1]); //Delta (variable length quantity)
            Assert.AreEqual(0x90, bytes[2]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[3]); //C4
            Assert.AreEqual(0x7F, bytes[4]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteNote(15, MidiNoteNumbers.C4, 127, 96)
            );

            //Assert           

            Assert.AreEqual("WriteNote must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfChannelAbove15()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            byte channel = 16;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNote(channel, MidiNoteNumbers.C4, 127, 96)
            );

            //Assert           

            Assert.AreEqual("channel must be in the range 0..15", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfVelocityAbove127()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            byte velocity = 128;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNote(0, MidiNoteNumbers.C4, velocity, 96)
            );

            //Assert           

            Assert.AreEqual("velocity must be in the range 0..127", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_Overload1ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteNote(0, MidiNoteNumbers.C4, 127, 100);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_Overload2ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteNote(0, new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 127, Duration = NoteDurations.QuarterNote } );

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }
    }
}
