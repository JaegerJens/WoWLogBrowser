using System;
using NUnit.Framework;
using LogIndex;

namespace LogIndexTests
{
    [TestFixture]
    public class TimecodeParserTests
    {
        [Test]
        public void LongTimecode()
        {
            // arrange
            var timecode = "12/21 19:55:03.332";
            var parser = new TimecodeParser(2014);

            // act
            var code = parser.Parse(timecode);

            // assert
            Assert.AreEqual(2014, code.Year);
            Assert.AreEqual(12, code.Month);
            Assert.AreEqual(21, code.Day);
            Assert.AreEqual(19, code.Hour);
            Assert.AreEqual(55, code.Minute);
            Assert.AreEqual(3, code.Second);
            Assert.AreEqual(332, code.Millisecond);
        }

        [Test]
        public void FullLineTimecodeParse()
        {
            // arrange
            var line = "12/21 20:07:57.357  SPELL_AURA_APPLIED,Player-3680-05FBE75B,\"Kalatos-Aman'Thul\",0x511,0x0,Player-3680-05FBE89D,\"Todesfaust-Aman'Thul\",0x10514,0x0,774,\"Rejuvenation\",0x8,BUFF";
            var parser = new TimecodeParser(2014);

            // act
            var code = parser.Parse(line);

            // assert
            Assert.AreEqual(2014, code.Year);
            Assert.AreEqual(12, code.Month);
            Assert.AreEqual(21, code.Day);
            Assert.AreEqual(20, code.Hour);
            Assert.AreEqual(7, code.Minute);
            Assert.AreEqual(57, code.Second);
            Assert.AreEqual(357, code.Millisecond);
        }
    }
}
