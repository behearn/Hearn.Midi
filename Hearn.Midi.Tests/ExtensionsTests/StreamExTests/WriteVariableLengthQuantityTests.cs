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
    public class WriteVariableLengthQuantityTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_0()
        {

            //Arrange
            var value = 0x00;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_1byte()
        {

            //Arrange
            var value = 0x7F;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            Assert.AreEqual(_stream.Length, 1);

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_127()
        {

            //Arrange
            var value = 0x7C;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x7C, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_2bytes()
        {

            //Arrange
            var value = 0x80;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            Assert.AreEqual(_stream.Length, 2);            

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_128()
        {

            //Arrange
            var value = 0x80;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_3bytes()
        {

            //Arrange
            var value = 0x4000;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            Assert.AreEqual(_stream.Length, 3);

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_16384()
        {

            //Arrange
            var value = 0x4000;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x80, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_4bytes()
        {

            //Arrange
            var value = 0x200000;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            Assert.AreEqual(_stream.Length, 4);

        }

        [TestMethod]
        public void StreamEx_WriteVariableLengthQuantity_2097152()
        {

            //Arrange
            var value = 0x200000;

            //Act
            _stream.WriteVariableLengthQuantity(value);

            //Assert
            _stream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(0x81, _stream.ReadByte());
            Assert.AreEqual(0x80, _stream.ReadByte());
            Assert.AreEqual(0x80, _stream.ReadByte());
            Assert.AreEqual(0x00, _stream.ReadByte());

        }
    }
}
