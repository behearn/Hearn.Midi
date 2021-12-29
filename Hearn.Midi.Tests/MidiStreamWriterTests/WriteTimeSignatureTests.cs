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
    public class WriteTimeSignatureTests
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
        public void MidiStreamWriter_WriteTimeSignature_MetaEventHeader()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(4, 4);

            //Assert           

            _stream.Seek(-8, SeekOrigin.End);
           
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0xFF, bytes[1]);
            Assert.AreEqual(0x58, bytes[2]);
            

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_TickConstants()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(4, 4);

            //Assert           

            _stream.Seek(-2, SeekOrigin.End);

            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x18, bytes[0]);
            Assert.AreEqual(0x08, bytes[1]);


        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_4x2()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(4, 2);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x04, bytes[0]);
            Assert.AreEqual(0x01, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_2x4()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(2, 4);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x02, bytes[0]);
            Assert.AreEqual(0x02, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_4x4()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(4, 4);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x04, bytes[0]);
            Assert.AreEqual(0x02, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_9x8()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(9, 8);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x09, bytes[0]);
            Assert.AreEqual(0x03, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_4x16()
        {

            //Arrange

            var bytes = new byte[2];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteTimeSignature(4, 16);

            //Assert           

            _stream.Seek(-4, SeekOrigin.End);
            _stream.Read(bytes, 0, 2);
            Assert.AreEqual(0x04, bytes[0]);
            Assert.AreEqual(0x04, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteTimeSignature(4, 4)
            );

            //Assert           

            Assert.AreEqual("WriteTimeSignature must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_ThrowsExceptionIfTopNumberZero()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteTimeSignature(0, 4)
            );

            //Assert           

            Assert.AreEqual("topNumber cannot be zero", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignature_ThrowsExceptionIfBottomNumberInvalid()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteTimeSignature(4, 6)
            );

            //Assert           

            Assert.AreEqual("Invalid bottomNumber (valid values are 2, 4, 8 & 16)", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteTimeSignatureo_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteTimeSignature(4, 4);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

    }
}
