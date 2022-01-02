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
    public class Format0SpecificationTests
    {

        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        [TestInitialize]
        public void TestInitialise()
        {

            //Common Arrange
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);
            
            //Common Act
            CreateMidiStream();
            
            //Tests assert different parts of the stream

        }

        private void CreateMidiStream()
        {

            _midiStreamWriter
                .WriteHeader(Formats.SingleTrack, 1)
                .WriteStartTrack()
                .WriteTimeSignature(4, 4)
                .WriteTempo(120)
                .WriteChangeInstrument(0, Instruments.ElectricPiano2)
                .WriteChangeInstrument(1, Instruments.OrchestralHarp)
                .WriteChangeInstrument(2, Instruments.Bassoon)
                .WriteNotes(2, new List<MidiNote>()
                {
                    new MidiNote() { Note = MidiNoteNumbers.C3, Velocity = 96, Duration =  NoteDurations.WholeNote },
                    new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 96, Duration =  NoteDurations.WholeNote }
                })
                .Tick(NoteDurations.QuarterNote)
                .WriteNote(1, new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = 64, Duration = NoteDurations.HalfNoteDotted })
                .Tick(NoteDurations.QuarterNote)
                .WriteNote(0, new MidiNote() { Note = MidiNoteNumbers.E5, Velocity = 32, Duration = NoteDurations.HalfNote })
                //Leave EndTrack to end playing notes in this test
                .WriteEndTrack();

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteHeader()
        {

            //4D 54 68 64 MThd
            //00 00 00 06 chunk length
            //00 00 format 0
            //00 01 one track
            //00 60 96 per quarter-note

            _stream.Seek(0, SeekOrigin.Begin);

            //MThd
            Assert.AreEqual(0x4D, _stream.ReadByte());
            Assert.AreEqual(0x54, _stream.ReadByte());
            Assert.AreEqual(0x68, _stream.ReadByte());
            Assert.AreEqual(0x64, _stream.ReadByte());

            //Chunk length
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x06, _stream.ReadByte());

            //Format 0
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            //1 Track
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x01, _stream.ReadByte());

            //96 per quarter note
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteStartTrack()
        {

            //4D 54 72 6B MTrk
            //00 00 00 3B chunk length(59)

            _stream.Seek(14, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x4D, _stream.ReadByte());
            Assert.AreEqual(0x54, _stream.ReadByte());
            Assert.AreEqual(0x72, _stream.ReadByte());
            Assert.AreEqual(0x6B, _stream.ReadByte());

            //Chunk length
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3B, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteTimeSignature()
        {

            //00 FF 58 04 04 02 18 08 time signature

            _stream.Seek(22, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x58, _stream.ReadByte());
            Assert.AreEqual(0x04, _stream.ReadByte());
            Assert.AreEqual(0x04, _stream.ReadByte());
            Assert.AreEqual(0x02, _stream.ReadByte());
            Assert.AreEqual(0x18, _stream.ReadByte());
            Assert.AreEqual(0x08, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteTempo()
        {

            //00 FF 51 03 07 A1 20 tempo

            _stream.Seek(30, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x51, _stream.ReadByte());
            Assert.AreEqual(0x03, _stream.ReadByte());
            Assert.AreEqual(0x07, _stream.ReadByte());
            Assert.AreEqual(0xA1, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());
            
        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteChangeInstrument()
        {

            //00 C0 05
            //00 C1 2E
            //00 C2 46

            _stream.Seek(37, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC0, _stream.ReadByte());
            Assert.AreEqual(0x05, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC1, _stream.ReadByte());
            Assert.AreEqual(0x2E, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC2, _stream.ReadByte());
            Assert.AreEqual(0x46, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteNotes()
        {

            //00 92 30 60
            //00 3C 60 running status

            _stream.Seek(46, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x92, _stream.ReadByte());
            Assert.AreEqual(0x30, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3C, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());


        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteNoteFirst()
        {

            //60 91 43 40

            _stream.Seek(53, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x60, _stream.ReadByte());
            Assert.AreEqual(0x91, _stream.ReadByte());
            Assert.AreEqual(0x43, _stream.ReadByte());
            Assert.AreEqual(0x40, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteNoteSecond()
        {

            //60 90 4C 20

            _stream.Seek(57, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x60, _stream.ReadByte());
            Assert.AreEqual(0x90, _stream.ReadByte());
            Assert.AreEqual(0x4C, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteEndTrackAutoEndNotes()
        {

            //81 40 - 82 30 00 two byte delta time
            //00 3C 00 running status
            //00 81 43 00
            //00 80 4C 00

            //Note : spec example has velocity (last byte) as 0x40 but we're writing 0x00

            _stream.Seek(61, SeekOrigin.Begin);

            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x40, _stream.ReadByte());
            Assert.AreEqual(0x82, _stream.ReadByte());
            Assert.AreEqual(0x30, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3C, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x43, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x80, _stream.ReadByte());
            Assert.AreEqual(0x4C, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteEndTrack()
        {

            //00 FF 2F 00 

            _stream.Seek(77, SeekOrigin.Begin);

            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x2F, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_DumpStream()
        {

            _stream.Seek(0, SeekOrigin.Begin);
            var buffer = _stream.GetBuffer();

            var midiFile = new FileStream("Format0SpecificationTests.mid", FileMode.OpenOrCreate);
            midiFile.Write(buffer, 0, (int)_stream.Length);
            midiFile.Flush();
            midiFile.Close();

        }

    }
}
