using LogIndex;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndexTests.Integration
{
    [TestFixture]
    public class WoWIndexLog
    {
        [Test]
        public void MythicFirstKillIndexTest()
        {
            // assert
            var filename = @"C:\Games\World of Warcraft\World of Warcraft\WorldOfLogs\WoWCombatLog.20141221.MH.MythicFirstKill.txt";
            var id = new Index(filename);

            // act
            id.CreateIndex();
        }
    }
}
