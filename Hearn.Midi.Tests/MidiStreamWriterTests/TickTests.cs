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
    public class TickTests
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
        public void MidiStreamWriter_Tick_StopsPlayingNote()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.Tick(NoteDurations.HalfNoteDotted);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End); //-4 for note off

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_Tick_LengthOverload()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .WriteNote(0, MidiNoteNumbers.C4, 127, 100);

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.Tick(150);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End); //-4 for note off

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x64, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_Tick_StopsPlayingMultipleNotes()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .WriteNotes(0, new List<MidiNote>()
                {
                    new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 127, Duration = NoteDurations.QuarterNote },
                    new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = 127, Duration = NoteDurations.QuarterNote }
                });

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.Tick(NoteDurations.HalfNote);

            //Assert           

            _stream.Seek(-7, SeekOrigin.End); //-3 for second note off, -4 for first note off

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x00, bytes[0]); //Delta (expect it to reset to zero here)
            Assert.AreEqual(0x43, bytes[1]); //G4
            Assert.AreEqual(0x00, bytes[2]); //Velocity


        }

        [TestMethod]
        public void MidiStreamWriter_Tick_StopsPlayingStaggeredNotes()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote)
                .WriteNote(0, MidiNoteNumbers.G4, 127, NoteDurations.HalfNote);
                
            var bytes = new byte[4];

            //Act

            _midiStreamWriter.Tick(NoteDurations.HalfNoteDotted);

            //Assert           

            _stream.Seek(-7, SeekOrigin.End); //-3 for second note off, -4 for first note off

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x60, bytes[0]); //Delta (stops a quarter note later)
            Assert.AreEqual(0x43, bytes[1]); //G4
            Assert.AreEqual(0x00, bytes[2]); //Velocity


        }

        [TestMethod]
        public void MidiStreamWriter_Tick_StopsPlayingNotesMultiChannel()
        {

            //Arrange

            _midiStreamWriter
                .WriteStartTrack()
                .WriteNote(0, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote)
                .WriteNote(1, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.Tick(NoteDurations.QuarterNote);

            //Assert           

            _stream.Seek(-8, SeekOrigin.End); //-4 for second note off, -4 for first note off

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //Delta
            Assert.AreEqual(0x80, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x81, bytes[1]); //Note off channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x00, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_Tick_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.Tick(NoteDurations.QuarterNote);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

        [TestMethod]
        public void MidiStreamWriter_Tick_OverloadReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.Tick(100);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

    }
}
