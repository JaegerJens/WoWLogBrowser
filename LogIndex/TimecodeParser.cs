using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndex
{
    public class TimecodeParser : ITimecodeParser
    {
        private const string format = "MM/dd HH:mm:ss.fff";

        public DateTime Parse(string line)
        {
            var timecode = line.Substring(0, 18);
            return DateTime.ParseExact(timecode, format, CultureInfo.InvariantCulture);
        }
    }
}
