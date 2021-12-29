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
    public class WritePercussionTests
    {
        MemoryStream _stream;

        MidiStreamWriter _midiStreamWriter;

        [TestInitialize]
        public void TestInitalise()
        {
            _stream = new MemoryStream();
            _midiStreamWriter = new MidiStreamWriter(_stream);

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2);

        }

        [TestMethod]
        public void MidiStreamWriter_WritePercussion_AcousticBassDrum()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WritePercussion(Percussion.AcousticBassDrum, 127);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]); //Delta
            Assert.AreEqual(0x99, bytes[1]); //Note on channel 10
            Assert.AreEqual(0x23, bytes[2]); //35 = AcousticBassDrum
            Assert.AreEqual(0x7F, bytes[3]); //Velocity

        }

        [TestMethod]
        public void MidiStreamWriter_WritePercussion_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WritePercussion(Percussion.AcousticBassDrum, 127);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }
    }
}
