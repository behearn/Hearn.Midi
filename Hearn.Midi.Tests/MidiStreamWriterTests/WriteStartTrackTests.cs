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
    public class WriteStartTracksTests
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
        public void MidiStreamWriter_WriteStartTrack_MTrk()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Act

            _midiStreamWriter.WriteStartTrack();

            //Assert           

            _stream.Seek(14, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 4);

            var header = Encoding.ASCII.GetString(bytes);
            Assert.AreEqual("MTrk", header);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_LengthPlaceholder()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.SingleTrack, 1);

            //Act

            _midiStreamWriter.WriteStartTrack();

            //Assert           

            _stream.Seek(18, SeekOrigin.Begin);
            _stream.Read(bytes, 0, 4);

            Assert.AreEqual(0x00, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
            Assert.AreEqual(0x00, bytes[3]); //Expect zero as not set until WriteEndTrack is called

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_ThrowsExceptionIfHeaderNotWritten()
        {

            //Arrange

            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteStartTrack()
            );

            //Assert           

            Assert.AreEqual("Create header before starting a new track", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_ThrowsExceptionIfPreviousTrackNotEnded()
        {

            //Arrange

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 3);

            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.WriteEndTrack();

            _midiStreamWriter.WriteStartTrack();

            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteStartTrack()
            );

            //Assert           

            Assert.AreEqual("End track 2 before starting a new track", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_ThrowsExceptionIfMaxTracksReached()
        {

            //Arrange

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2);

            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.WriteEndTrack();

            _midiStreamWriter.WriteStartTrack();
            _midiStreamWriter.WriteEndTrack();

            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteStartTrack()
            );

            //Assert           

            Assert.AreEqual("Max number of tracks reached", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2);

            //Act

            var msw = _midiStreamWriter.WriteStartTrack();

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }
        
        [TestMethod]
        public void MidiStreamWriter_WriteStartTrack_CurrentTrackSet()
        {

            //Arrange

            _midiStreamWriter
                .WriteHeader(MidiStreamWriter.Formats.MultiSimultaneousTracks, 2)
                .WriteStartTrack()
                .WriteEndTrack();


            //Act

            _midiStreamWriter.WriteStartTrack();

            //Assert           

            Assert.AreEqual(2, _midiStreamWriter.CurrentTrack);

        }
    }
}
