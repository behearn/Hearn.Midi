using Hearn.Midi.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearn.Midi.Tests.ExtensionsTests.LongExTests
{
    [TestClass]
    public class ToVariableLengthQuantityTests
    {

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_0()
        {

            //Arrange
            var value = 0L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_00000000, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_127()
        {

            //Arrange
            var value = 127L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_01111111, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_128()
        {

            //Arrange
            var value = 128L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_10000001_00000000, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_8192()
        {

            //Arrange
            var value = 8192L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_11000000_00000000, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_16383()
        {

            //Arrange
            var value = 16383L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_11111111_01111111, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_16384()
        {

            //Arrange
            var value = 16384L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_10000001_10000000_00000000, actual);

        }
        
        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_2097151()
        {

            //Arrange
            var value = 2097151L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_11111111_11111111_01111111, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_2097152()
        {

            //Arrange
            var value = 2097152L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_10000001_10000000_10000000_00000000, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_134217728()
        {

            //Arrange
            var value = 134217728L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_11000000_10000000_10000000_00000000, actual);

        }

        [TestMethod]
        public void LongEx_ToVariableLengthQuantity_268435455()
        {

            //Arrange
            var value = 268435455L;

            //Act
            var actual = value.ToVariableLengthQuantity();

            //Assert
            Assert.AreEqual(0b_11111111_11111111_11111111_01111111, actual);

        }

    }
}
