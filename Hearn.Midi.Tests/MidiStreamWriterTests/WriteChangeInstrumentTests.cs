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
    public class WriteChangeInstrumentTests
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
        public void MidiStreamWriter_WriteChangeInstrument_Channel1()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteChangeInstrument(0, Instruments.Accordion);

            //Assert           

            _stream.Seek(-3, SeekOrigin.End); 
           
            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x00, bytes[0]); //Delta time
            Assert.AreEqual(0xC0, bytes[1]); //Change programme channel 1 [0]
            Assert.AreEqual(0x15, bytes[2]); //Accordion (21)
            
        }

        [TestMethod]
        public void MidiStreamWriter_WriteChangeInstrument_Channel16()
        {

            //Arrange

            var bytes = new byte[4];

            _midiStreamWriter.WriteStartTrack();

            //Act

            _midiStreamWriter.WriteChangeInstrument(15, Instruments.Xylophone);

            //Assert           

            _stream.Seek(-3, SeekOrigin.End);

            _stream.Read(bytes, 0, 3);
            Assert.AreEqual(0x00, bytes[0]); //Delta time
            Assert.AreEqual(0xCF, bytes[1]); //Change programme channel 16 [15]
            Assert.AreEqual(0x0D, bytes[2]); //Xylophone (13)

        }

        [TestMethod]
        public void MidiStreamWriter_WriteChangeInstrument_ThrowsExceptionIfOutsideTrack()
        {

            //Arrange


            //Act

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                _midiStreamWriter.WriteChangeInstrument(0, Instruments.AcousticGrandPiano)
            );

            //Assert           

            Assert.AreEqual("WriteChangeInstrument must be called after WriteStartTrack", ex.Message);

        }

        [TestMethod]
        public void MidiStreamWriter_WriteString_ReturnsMidiStreamWriter()
        {

            //Arrange

            _midiStreamWriter.WriteStartTrack();

            //Act

            var msw = _midiStreamWriter.WriteChangeInstrument(0, Instruments.AcousticGrandPiano);

            //Assert           

            Assert.AreEqual(msw, _midiStreamWriter);

        }

    }
}
