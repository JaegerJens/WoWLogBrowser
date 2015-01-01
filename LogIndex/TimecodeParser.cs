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
        public int Year { get; set; }

        public TimecodeParser(int year)
        {
            Year = year;
        }

        public DateTime Parse(string line)
        {
            var timecode = line.Substring(0, 18);
            var dt = DateTime.ParseExact(timecode, format, CultureInfo.InvariantCulture);
            if (Year > 1900)
            {
                var diff = Year - dt.Year;
                return dt.AddYears(diff);
            }
            return dt;
        }
    }
}
