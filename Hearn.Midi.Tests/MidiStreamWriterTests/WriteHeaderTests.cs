using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Tests.MidiStreamWriterTests
{
    [TestClass]
    public class WriteHeaderTests
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
        public void MidiStreamWriter_WriteHeader_MThd()
        {

            //Arrange

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Assert

            _stream.Seek(0, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 4);

            var header = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual("MThd", header);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_HeaderLengthIs6()
        {

            //Arrange

            var bytes = new byte[4];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Assert

            _stream.Seek(4, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x06, bytes[3]);


        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_Format0()
        {

            //Arrange

            var bytes = new byte[2];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Assert

            _stream.Seek(8, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 2);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_Format1()
        {

            //Arrange

            var bytes = new byte[2];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2);

            //Assert

            _stream.Seek(8, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 2);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x01, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_Format2()
        {

            //Arrange

            var bytes = new byte[2];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSequentialTracks, 2);

            //Assert

            _stream.Seek(8, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 2);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x02, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_Tracks()
        {

            //Arrange

            var bytes = new byte[2];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 4);

            //Assert

            _stream.Seek(10, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 2);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x04, bytes[1]);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_TicksPerQuarterNote()
        {

            //Arrange

            var bytes = new byte[2];

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Assert

            _stream.Seek(12, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 2);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]); //96

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_ZeroTracksThrowsException()
        {

            //Arrange

            byte tracks = 0;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, tracks)
            );

            //Assert

            Assert.AreEqual("tracks cannot be zero", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_SingleTrackThrowsExceptionForMoreThanOneTrack()
        {

            //Arrange

            byte tracks = 2;

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, tracks)
            );

            //Assert

            Assert.AreEqual("tracks must be 1 when format is SingleTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_MultiSimultaneousTracksAllowed()
        {

            //Arrange

            byte tracks = 2;

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, tracks);

            //Assert

            Assert.AreEqual(2, _midiStreamWriter.Tracks);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_MultiSequentialTracksAllowed()
        {

            //Arrange

            byte tracks = 2;

            //Act

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSequentialTracks, tracks);

            //Assert

            Assert.AreEqual(2, _midiStreamWriter.Tracks);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteHeader_ThrowsExceptionIfCalledAgain()
        {

            //Arrange
            
            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1)
            );

            //Assert

            Assert.AreEqual("WriteHeader has already been called", ex.Message);

        }


        [TestMethod]
        public void MidiStreamWriter_WriteStartHeader_ReturnsMidiStreamWriter()
        {

            //Arrange
            
            //Act

            var msw = _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

    }
}
