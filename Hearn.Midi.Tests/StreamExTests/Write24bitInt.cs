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
    public class Write24bitIntTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_Write24bitInt_3bytes()
        {

            //Arrange
            var value = 0xFFFFFF;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            Assert.AreEqual(_stream.Length, 3);

        }

        [TestMethod]
        public void StreamEx_Write24bitInt_0()
        {

            //Arrange
            var value = 0x00000000;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_Write24bitInt_127()
        {

            //Arrange
            var value = 0x00007F;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x7F, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_Write24bitInt_8192()
        {

            //Arrange
            var value = 0x002000;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_Write24bitInt_16383()
        {

            //Arrange
            var value = 0x003FFF;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_Write24bitInt_2097151()
        {

            //Arrange
            var value = 0x1FFFFF;

            //Act
            _stream.Write24bitInt(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x1F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }

    }
}
