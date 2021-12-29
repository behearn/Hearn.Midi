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
    public class WriteTempoTests
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
        public void MidiStreamWriter_WriteTempo_MetaEventHeader()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTempo(128);

            //Assert           

            _stream.Seek(-7, SeekOrigin.End);
           
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0xFF, bytes[1]);
            Assert.AreEqual(0x51, bytes[2]);
            Assert.AreEqual(0x03, bytes[3]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTempo_Value()
        {

            //Arrange

            var bytes = new byte[3];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTempo(128);

            //60,000,000 / 128 = 468,750 = 0x07270E

            //Assert           

            _stream.Seek(-3, SeekOrigin.End);
            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x07, bytes[0]);
            Assert.AreEqual(0x27, bytes[1]);
            Assert.AreEqual(0x0E, bytes[2]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTempo_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteTempo(128)
            );

            //Assert           

            Assert.AreEqual("WriteTempo must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTempo_ThrowsExceptionIfZero()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteTempo(0)
            );

            //Assert           

            Assert.AreEqual("Tempo cannot be zero", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTempo_ThrowsExceptionIfNegative()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteTempo(-1)
            );

            //Assert           

            Assert.AreEqual("Tempo cannot be negative", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTempo_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteTempo(128);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

    }
}
