using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndex
{
    public class LineSplicer
    {
        private readonly byte CarriageReturn;
        private readonly byte LineFeed;

        public LineSplicer()
        {
            CarriageReturn = 13;
            LineFeed = 10;
        }

        public int GetPosOfEndOfLine(ref byte[] data)
        {
            for (var p = 0; p < data.Length; p++)
            {
                if ((data[p] == LineFeed) || (data[p] == CarriageReturn))
                {
                    return p;
                }
            }
            throw new Exception("end of line not found");
        }

        public string GetNextLine(ref byte[] buffer, ref int offset, int bufferLen)
        {
            if (buffer[offset] == CarriageReturn)
                offset++;
            if ((buffer[offset] == LineFeed) || (buffer[offset] == LineFeed))
                offset++;

            var lineLen = bufferLen - offset;
            var lineSeg = new ArraySegment<byte>(buffer, offset, lineLen);
            var lineBuf = lineSeg.ToArray();
            var text = Encoding.ASCII.GetString(lineBuf);
            return text;
        }
    }
}
