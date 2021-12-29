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
    public class WriteStringTests
    {
        const string TRACK_NAME = "Unit test track!";

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
        public void MidiStreamWriter_WriteString_Header()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteString(StringTypes.TrackName, TRACK_NAME);

            //Assert           

            _stream.Seek(-20, SeekOrigin.End); //16 for text, -4 for text meta event
           
            _stream.Read(bytes, 0, 4);
            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0xFF, bytes[1]);
            Assert.AreEqual(0x03, bytes[2]); //Track name
            Assert.AreEqual(0x10, bytes[3]); //string length 16

        }

        [TestMethod]
        public void MidiStreamWriter_WriteString_Text()
        {

            //Arrange

            var bytes = new byte[16];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteString(StringTypes.TrackName, TRACK_NAME);

            //Assert           

            _stream.Seek(-16, SeekOrigin.End); //-16 for text
            _stream.Read(bytes, 0, 16);

            var text = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual(TRACK_NAME, text);
        }

        [TestMethod]
        public void MidiStreamWriter_WriteString_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteString(StringTypes.TrackName, TRACK_NAME)
            );

            //Assert           

            Assert.AreEqual("WriteString must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteString_ThrowsExceptionIfTextTooLong()
        {

            //Arrange
            
            var longText = new string('x', 128);

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<ArgumentException>(() =>
                _midiStreamWriter.WriteString(StringTypes.TrackName, longText)
            );

            //Assert           

            Assert.AreEqual("text exceeds 127 characters", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteString_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteString(StringTypes.TrackName, TRACK_NAME);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }



    }
}
