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
    public class ReadVariableLengthQuantityTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_0()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value =  _stream.ReadVariableLengthQuantity();

            //Assert
            
            Assert.AreEqual(0, value);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_1byte()
        {


            //Arrange

            _stream.WriteByte(0x7F);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(1, _stream.Position);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_127()
        {


            //Arrange

            _stream.WriteByte(0x7F);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(127, value);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_2bytes()
        {


            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(2, _stream.Position);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_128()
        {

            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(128, value);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_3bytes()
        {

            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(3, _stream.Position);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_16384()
        {

            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(16384, value);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_4bytes()
        {

            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(4, _stream.Position);

        }

        [TestMethod]
        public void StreamEx_ReadVariableLengthQuantity_2097152()
        {

            //Arrange

            _stream.WriteByte(0x81);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x80);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadVariableLengthQuantity();

            //Assert

            Assert.AreEqual(2097152, value);

        }
    }
}
