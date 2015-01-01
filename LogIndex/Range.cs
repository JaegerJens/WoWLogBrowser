using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndex
{
    public class Range
    {
        public Position Start { get; set; }
        public Position End { get; set; }

        public long GetOffsetDiff()
        {
            return End.Offset - Start.Offset;
        }
    }
}
