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
    public class Read24bitIntTests
    {

        MemoryStream _stream;

        [TestInitialize]
        public void TestInitialise()
        {
            _stream = new MemoryStream();
        }

        [TestMethod]
        public void StreamEx_Read24bitInt_3bytes()
        {

            //Arrange

            _stream.WriteByte(0x01);
            _stream.WriteByte(0x02);
            _stream.WriteByte(0x03);
            _stream.WriteByte(0x04);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(3, _stream.Position);

        }

        [TestMethod]
        public void StreamEx_Read24bitInt_0()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(0, value);

        }

        [TestMethod]
        public void StreamEx_Read24bitInt_127()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x00);
            _stream.WriteByte(0x7F);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(127, value);

        }

        [TestMethod]
        public void StreamEx_Read24bitInt_8192()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x20);
            _stream.WriteByte(0x00);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(8192, value);

        }

        [TestMethod]
        public void StreamEx_Read24bitInt_16383()
        {

            //Arrange

            _stream.WriteByte(0x00);
            _stream.WriteByte(0x3F);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(16383, value);

        }

        [TestMethod]
        public void StreamEx_Read24bitInt_2097151()
        {

            //Arrange

            _stream.WriteByte(0x1F);
            _stream.WriteByte(0xFF);
            _stream.WriteByte(0xFF);
            _stream.Seek(0, SeekOrigin.Begin);

            //Act

            var value = _stream.Read24bitInt();

            //Assert

            Assert.AreEqual(2097151, value);

        }

    }
}
