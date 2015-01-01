using LogIndex;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndexTests
{
    [TestFixture]
    public class LineSplicerTests
    {
        [Test]
        public void SimpleWindowsEndOfLineTest()
        {
            // arrange
            var text = "Hello World\r\nAnd Welcome\r\n";
            var buffer = Encoding.UTF8.GetBytes(text);
            var splicer = new LineSplicer();

            // act
            var eol = splicer.GetPosOfEndOfLine(ref buffer);

            // assert
            Assert.AreEqual(11, eol);
        }

        [Test]
        public void SimpleUnixEndOfLineTest()
        {
            // arrange
            var text = "Hello World\nAnd Welcome\n";
            var buffer = Encoding.UTF8.GetBytes(text);
            var splicer = new LineSplicer();

            // act
            var eol = splicer.GetPosOfEndOfLine(ref buffer);

            // assert
            Assert.AreEqual(11, eol);
        }

        [Test]
        public void WindowsNextLineTest()
        {
            // arrange
            var text = "Hello World\r\nAnd Welcome\r\n";
            var buffer = Encoding.UTF8.GetBytes(text);
            var splicer = new LineSplicer();
            var eol = splicer.GetPosOfEndOfLine(ref buffer);

            // act
            var result = splicer.GetNextLine(ref buffer, ref eol, buffer.Length);

            // assert
            Assert.AreEqual(13, eol);
            Assert.AreEqual("And Welcome\r\n", result);
        }

        [Test]
        public void UnixNextLineTest()
        {
            // arrange
            var text = "Hello World\nAnd Welcome\n";
            var buffer = Encoding.UTF8.GetBytes(text);
            var splicer = new LineSplicer();
            var eol = splicer.GetPosOfEndOfLine(ref buffer);

            // act
            var result = splicer.GetNextLine(ref buffer, ref eol, buffer.Length);

            // assert
            Assert.AreEqual(12, eol);
            Assert.AreEqual("And Welcome\n", result);
        }
    }
}
