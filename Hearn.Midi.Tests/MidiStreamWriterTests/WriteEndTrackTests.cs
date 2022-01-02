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
    public class WriteEndTracksTests
    {
        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        [TestInitialize]
        public void TestInitalise()
        {
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);            
        }

        [TestMethod]
        public void MidiStreamWriter_WriteEndTrack_EndTrackMetaEvent()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteHeader(Formats.SingleTrack, 1);
            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.WriteNote(1, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Act

            _midiStreamWriter.WriteEndTrack();

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0xFF, bytes[1]);
            Assert.AreEqual(0x2F, bytes[2]);
            Assert.AreEqual(0x00, bytes[3]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteEndTrack_LengthUpdated()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteHeader(Formats.SingleTrack, 1);
            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.WriteNote(1, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Act

            _midiStreamWriter.WriteEndTrack();

            //Assert           

            _stream.Seek(-16, SeekOrigin.End); // -4 for end track meta event, -4 for note on, -4 for note off, -4 for length
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x0C, bytes[3]); //4 for note on, 4 for note off, 4 end track meta event

        }

        [TestMethod]
        public void MidiStreamWriter_WriteEndTrack_StopsPlayingNotes()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteHeader(Formats.SingleTrack, 1)
                .WriteStartTrack()
                .WriteNoteAndTick(1, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote);

            //Act

            _midiStreamWriter
                .WriteNote(2, MidiNoteNumbers.C4, 127, NoteDurations.HalfNoteDotted)
                .Tick(NoteDurations.QuarterNote)
                .WriteNote(3, MidiNoteNumbers.C4, 127, NoteDurations.QuarterNote)
                .WriteEndTrack();

            //1:X---
            //2:-XXX 
            //3:--X- <--- this note gets stopped first
            //    ^ current position
            //Notes on channels 2 & 3 have not been stoppped as Tick has not been called before end track

            //Assert           

            _stream.Seek(-12, SeekOrigin.End); // -4 for end track meta event, -4 for note off, -4 for second note off
            
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //delta time (move on 96 ticks / quarter note)
            Assert.AreEqual(0x83, bytes[1]); //note off channel 3
            Assert.AreEqual(0x3C, bytes[2]); //Note C4
            Assert.AreEqual(0x00, bytes[3]); //0 velocity

            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x60, bytes[0]); //delta time (move on 96 ticks / quarter note)
            Assert.AreEqual(0x82, bytes[1]); //note off channel 2
            Assert.AreEqual(0x3C, bytes[2]); //Note C4
            Assert.AreEqual(0x00, bytes[3]); //0 velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WriteEndTrack_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteHeader(Formats.MultiSimultaneousTracks, 2);

            //Act

            var msw = _midiStreamWriter.WriteStartTrack();

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_CurrentTrackReset()
        {

            //Arrange

            _midiStreamWriter
                .WriteHeader(Formats.MultiSimultaneousTracks, 2)
                .WriteStartTrack();


            //Act

            _midiStreamWriter.WriteEndTrack();

            //Assert           

            Assert.AreEqual(-1, _midiStreamWriter.CurrentTrack);

        }


    }
}
