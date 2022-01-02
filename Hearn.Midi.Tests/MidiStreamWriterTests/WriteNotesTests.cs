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
    public class WriteNotesTests
    {
        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        List<MidiNote> _chord;

        [TestInitialize]
        public void TestInitalise()
        {
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);

            _midiStreamWriter.WriteHeader(Formats.MultiSimultaneousTracks, 2);

            _chord = new List<MidiNote>()
            {
                new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 127, Duration = NoteDurations.WholeNote },
                new MidiNote() { Note = MidiNoteNumbers.E4, Velocity = 127, Duration = NoteDurations.WholeNote },
                new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = 127, Duration = NoteDurations.WholeNote }
            };
        }

        [TestMethod]
        public void MidiStreamWriter_WriteNotes_CMajorChord()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteNotes(0, _chord);

            //Assert           

            _stream.Seek(-10, SeekOrigin.End); //-4 for first note, -3 each for next two (no note on event)
            
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x90, bytes[1]); //Note on channel 1
            Assert.AreEqual(0x3C, bytes[2]); //C4
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x40, bytes[1]); //E4
            Assert.AreEqual(0x7F, bytes[2]); //Velocity
            
            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x43, bytes[1]); //G4
            Assert.AreEqual(0x7F, bytes[2]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteNotes(0, _chord)
            );

            //Assert           

            Assert.AreEqual("WriteNotes must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfChannelAbove15()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            byte channel = 16;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNotes(channel, _chord)
            );

            //Assert           

            Assert.AreEqual("channel must be in the range 0..15", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNotes_ThrowsExceptionIfVelocityAbove127()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            _chord[0].Velocity = 128;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNotes(0, _chord)
            );

            //Assert           

            Assert.AreEqual("velocity must be in the range 0..127", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfNotesNull()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNotes(0, null)
            );

            //Assert           

            Assert.AreEqual("midiNotes not supplied", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteNote_ThrowsExceptionIfNotesEmpty()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteNotes(0, new List<MidiNote>())
            );

            //Assert           

            Assert.AreEqual("midiNotes not supplied", ex.Message);

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

    }
}
