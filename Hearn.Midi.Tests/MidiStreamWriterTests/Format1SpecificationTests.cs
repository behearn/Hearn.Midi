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
    public class Format1SpecificationTests
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
                .WriteHeader(Formats.MultiSimultaneousTracks, 4);

            _midiStreamWriter
                .WriteStartTrack()
                .WriteTimeSignature(4, 4)
                .WriteTempo(120)
                .WriteEndTrack();

            _midiStreamWriter
                .WriteStartTrack()
                .WriteChangeInstrument(0, Instruments.ElectricPiano2)
                .Wait(NoteDurations.HalfNote)
                .WriteNote(0, new MidiNote() { Note = MidiNoteNumbers.E5, Velocity = 32, Duration = NoteDurations.HalfNote })
                .WriteEndTrack();

            _midiStreamWriter
                .WriteStartTrack()
                .WriteChangeInstrument(1, Instruments.OrchestralHarp)
                .Wait(NoteDurations.QuarterNote)
                .WriteNote(1, new MidiNote() { Note = MidiNoteNumbers.G4, Velocity = 64, Duration = NoteDurations.HalfNoteDotted })
                .WriteEndTrack();

            _midiStreamWriter
                .WriteStartTrack()
                .WriteChangeInstrument(2, Instruments.Bassoon)
                .WriteNotes(2, new List<MidiNote>()
                {
                    new MidiNote() { Note = MidiNoteNumbers.C3, Velocity = 96, Duration =  NoteDurations.WholeNote },
                    new MidiNote() { Note = MidiNoteNumbers.C4, Velocity = 96, Duration =  NoteDurations.WholeNote }
                })
                .WriteEndTrack();

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_WriteHeader()
        {

            //4D 54 68 64 MThd
            //00 00 00 06 chunk length
            //00 01 format 1
            //00 04 four tracks
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

            //Format 1
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x01, _stream.ReadByte());

            //4 Track
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x04, _stream.ReadByte());

            //96 per quarter note
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_Track1()
        {

            //4D 54 72 6B MTrk
            //00 00 00 13 chunk length(20) 
            //00 FF 58 04 04 02 18 08 time signature
            //00 FF 51 03 07 A1 20 tempo
            //00 FF 2F 00 end of track

            //Note : spec has chunk length 0x14 and a delta time of 0x83 0x00 for the end track

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
            Assert.AreEqual(0x13, _stream.ReadByte()); //Spec has 0x14

            //Time signature
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x58, _stream.ReadByte());
            Assert.AreEqual(0x04, _stream.ReadByte());
            Assert.AreEqual(0x04, _stream.ReadByte());
            Assert.AreEqual(0x02, _stream.ReadByte());
            Assert.AreEqual(0x18, _stream.ReadByte());
            Assert.AreEqual(0x08, _stream.ReadByte());

            //Tempo
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x51, _stream.ReadByte());
            Assert.AreEqual(0x03, _stream.ReadByte());
            Assert.AreEqual(0x07, _stream.ReadByte());
            Assert.AreEqual(0xA1, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());

            //End of track
            Assert.AreEqual(0x00, _stream.ReadByte()); //Spec has 0x84 0x00
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x2F, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_Track2()
        {

            //4D 54 72 6B MTrk
            //00 00 00 10
            //00 C0 05
            //81 40 90 4C 20
            //81 40 4C 00 Running status: note on, vel = 0
            //00 FF 2F 00

            //Note : Spec has chunk length of 0x10, increased to 0x11 due to extra 0x80 note off event

            _stream.Seek(41, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x4D, _stream.ReadByte());
            Assert.AreEqual(0x54, _stream.ReadByte());
            Assert.AreEqual(0x72, _stream.ReadByte());
            Assert.AreEqual(0x6B, _stream.ReadByte());

            //Chunk length
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x11, _stream.ReadByte()); //Spec has 0x10

            //Change program
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC0, _stream.ReadByte());
            Assert.AreEqual(0x05, _stream.ReadByte());

            //Note on
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x40, _stream.ReadByte());
            Assert.AreEqual(0x90, _stream.ReadByte());
            Assert.AreEqual(0x4C, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());

            //Note off
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x40, _stream.ReadByte());
            Assert.AreEqual(0x80, _stream.ReadByte()); //Additional to spec
            Assert.AreEqual(0x4C, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            //End of track
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x2F, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_Track3()
        {

            //4D 54 72 6B MTrk
            //00 00 00 0F
            //00 C1 2E
            //60 91 43 40
            //82 20 43 00 running status
            //00 FF 2F 00

            //Note : Spec has chunk length of 0x0F, increased to 0x10 due to extra 0x81 note off event

            _stream.Seek(66, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x4D, _stream.ReadByte());
            Assert.AreEqual(0x54, _stream.ReadByte());
            Assert.AreEqual(0x72, _stream.ReadByte());
            Assert.AreEqual(0x6B, _stream.ReadByte());

            //Chunk length
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x10, _stream.ReadByte()); //Spec has 0x0F

            //Change program
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC1, _stream.ReadByte());
            Assert.AreEqual(0x2E, _stream.ReadByte());

            //Note on
            Assert.AreEqual(0x60, _stream.ReadByte());
            Assert.AreEqual(0x91, _stream.ReadByte());
            Assert.AreEqual(0x43, _stream.ReadByte());
            Assert.AreEqual(0x40, _stream.ReadByte());

            //Note off
            Assert.AreEqual(0x82, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());
            Assert.AreEqual(0x81, _stream.ReadByte()); //Additional to spec
            Assert.AreEqual(0x43, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            //End of track
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0x2F, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void MidiStreamWriter_Specification_Track4()
        {

            //4D 54 72 6B MTrk
            //00 00 00 15
            //00 C2 46 
            //00 92 30 60
            //00 3C 60 running status
            //83 00 30 00 two - byte delta - time, running status
            //00 3C 00 running status
            //00 FF 2F 00

            //Note : Spec has chunk length of 0x15, increased to 0x16 due to extra 0x82 note off event

            _stream.Seek(90, SeekOrigin.Begin);

            //MTrk
            Assert.AreEqual(0x4D, _stream.ReadByte());
            Assert.AreEqual(0x54, _stream.ReadByte());
            Assert.AreEqual(0x72, _stream.ReadByte());
            Assert.AreEqual(0x6B, _stream.ReadByte());

            //Chunk length
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x16, _stream.ReadByte()); //Spec has 0x15

            //Change program
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0xC2, _stream.ReadByte());
            Assert.AreEqual(0x46, _stream.ReadByte());

            //Note on
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x92, _stream.ReadByte());
            Assert.AreEqual(0x30, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());

            //Note on
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3C, _stream.ReadByte());
            Assert.AreEqual(0x60, _stream.ReadByte());

            //Note off
            Assert.AreEqual(0x83, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x82, _stream.ReadByte()); //Additional to spec
            Assert.AreEqual(0x30, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            //Note off
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3C, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

            //End of track
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

            var midiFile = new FileStream("Format1SpecificationTests.mid", FileMode.OpenOrCreate);
            midiFile.Write(buffer, 0, (int)_stream.Length);
            midiFile.Flush();
            midiFile.Close();

        }

    }
}
