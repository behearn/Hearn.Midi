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
    public class ReadIntTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_ReadInt_2bytes()
        {

            //Arrange

            _stream.WriteByte(0x01);
            _stream.WriteByte(0x02);
            _stream.WriteByte(0x03);
            _stream.WriteByte(0x04);

            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadInt();

            //Assert

            Assert.AreEqual(_stream.Position, 2);

        }

        [TestMethod]
        public void StreamEx_ReadInt_0()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadInt();

            //Assert

            Assert.AreEqual(0, value);

        }

        [TestMethod]
        public void StreamEx_ReadInt_127()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x7F);
            _stream.Seek(0, SeekOrigin.Begin);
            
            //Act

            var value = _stream.ReadInt();

            //Assert
            
            Assert.AreEqual(127, value);

        }

        [TestMethod]
        public void StreamEx_ReadInt_8192()
        {

            //Arrange

            _stream.WriteByte(0x20);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadInt();

            //Assert
            
            Assert.AreEqual(8192, value);

        }

        [TestMethod]
        public void StreamEx_ReadInt_16383()
        {

            //Arrange
            
            _stream.WriteByte(0x3F);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadInt();

            //Assert
            
            Assert.AreEqual(16383, value);            

        }
    }
}
