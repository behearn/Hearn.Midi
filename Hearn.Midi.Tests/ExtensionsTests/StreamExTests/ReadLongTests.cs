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
    public class ReadLongTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_ReadLong_4bytes()
        {

            //Arrange

            _stream.WriteByte(0x01);
            _stream.WriteByte(0x02);
            _stream.WriteByte(0x03);
            _stream.WriteByte(0x04);
            _stream.WriteByte(0x05);
            _stream.WriteByte(0x06);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(_stream.Position, 4);

        }

        [TestMethod]
        public void StreamEx_ReadLong_0()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(0, value);

        }

        [TestMethod]
        public void StreamEx_ReadLong_127()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x7F);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(127, value);

        }

        [TestMethod]
        public void StreamEx_ReadLong_8192()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x20);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(8192, value);

        }

        [TestMethod]
        public void StreamEx_ReadLong_16383()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x3F);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(16383, value);

        }

        [TestMethod]
        public void StreamEx_ReadLong_2097151()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x1F);
            _stream.WriteByte(0xFF);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(2097151, value);

        }

        [TestMethod]
        public void StreamEx_ReadLong_268435455()
        {

            //Arrange

            _stream.WriteByte(0x0F);
            _stream.WriteByte(0xFF);
            _stream.WriteByte(0xFF);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.ReadLong();

            //Assert

            Assert.AreEqual(268435455, value);

        }
    }
}
