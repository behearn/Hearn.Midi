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
    public class WriteControlChangeTests
    {
        
        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        [TestInitialize]
        public void TestInitalise()
        {
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);

            _midiStreamWriter.WriteHeader(Formats.SingleTrack, 1);
        }

        [TestMethod]
        public void MidiStreamWriter_WriteControlChange_Channel0Delta0Attack96()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteControlChange(0, ControlChangeTypes.SoundController4AttackTimeLSB, 96);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0xB0, bytes[1]);
            Assert.AreEqual(0x49, bytes[2]);
            Assert.AreEqual(0x60, bytes[3]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteControlChange_Channel1Delta128Attack48()
        {

            //Arrange

            var bytes = new byte[5];

            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.Tick(128);

            //Act

            _midiStreamWriter.WriteControlChange(1, ControlChangeTypes.SoundController3ReleaseTimeLSB, 48);

            //Assert           

            _stream.Seek(-5, SeekOrigin.End);
            _stream.Read(bytes, 0, 5);
            Assert.AreEqual(0x81, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0xB1, bytes[2]);
            Assert.AreEqual(0x48, bytes[3]);
            Assert.AreEqual(0x30, bytes[4]);

        }
    }
}
