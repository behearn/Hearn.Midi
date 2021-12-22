using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Tests.StreamExTests
{
    [TestClass]
    public class WriteIntTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_WriteInt_2bytes()
        {

            //Arrange
            var value = 0xFFFF;

            //Act
            _stream.WriteInt(value);

            //Assert
            Assert.AreEqual(_stream.Length, 2);

        }

        [TestMethod]
        public void StreamEx_WriteInt_0()
        {

            //Arrange
            var value = 0x0000;

            //Act
            _stream.WriteInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_127()
        {

            //Arrange
            var value = 0x007F;

            //Act
            _stream.WriteInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x7F, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_8192()
        {

            //Arrange
            var value = 0x2000;

            //Act
            _stream.WriteInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x20, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_16383()
        {

            //Arrange
            var value = 0x3FFF;

            //Act
            _stream.WriteInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x3F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }
    }
}
