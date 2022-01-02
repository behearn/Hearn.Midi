using Hearn.Midi.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Tests.ExtensionsTests.StreamExTests
{
    [TestClass]
    public class WriteLongTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_WriteLong_4bytes()
        {

            //Arrange
            var value = 0xFFFFFFFF;

            //Act
            _stream.WriteLong(value);

            //Assert
            Assert.AreEqual(_stream.Length, 4);

        }

        [TestMethod]
        public void StreamEx_WriteLong_0()
        {

            //Arrange
            var value = 0x00000000;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_127()
        {

            //Arrange
            var value = 0x0000007F;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x7F, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_8192()
        {

            //Arrange
            var value = 0x00002000;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x20, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_16383()
        {

            //Arrange
            var value = 0x00003FFF;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x3F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_2097151()
        {

            //Arrange
            var value = 0x001FFFFF;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());
            Assert.AreEqual(0x1F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteInt_268435455()
        {

            //Arrange
            var value = 0x0FFFFFFF;

            //Act
            _stream.WriteLong(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x0F, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());
            Assert.AreEqual(0xFF, _stream.ReadByte());

        }
    }
}
