using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogIndex
{
    public class Index
    {
        private List<Position> _index;

        public Index()
        {
            Interval = 4 * 1024 * 1024; // 4 MB
            Parser = new TimecodeParser(DateTime.Now.Year);
            Splicer = new LineSplicer();
        }

        public Index(string Filename)
            :this()
	    {
            LogFile = new FileInfo(Filename);
            Parser = new TimecodeParser(LogFile.CreationTime.Year);
            if (!LogFile.Exists)
                throw new ArgumentException("file does not exists");
        }
	    
        public FileInfo LogFile { get; private set; }
        public ITimecodeParser Parser { get; set; }
        public LineSplicer Splicer { get; set; }
        public int Interval { get; set; }
        private const int bufLen = 1024;

        public void CreateIndex()
        {
            if (Parser == null)
                throw new Exception("no TimecodeParser found");
            if (Interval < 20)
                throw new Exception("invalid Intervall");

            _index = new List<Position>();

            using (var fh =  LogFile.OpenRead())
            {
                var sr = new StreamReader(fh);
                long seekPos = 0;
                byte[] buffer = new byte[bufLen];
                while(true)
                {
                    // read buffer on seek position
                    fh.Seek(seekPos, 0);
                    var readDone = fh.Read(buffer, 0, bufLen);
                    if (readDone == 0)
                    {
                        break;
                    }

                    // find and parse next text line
                    int eolPos;
                    if (seekPos == 0)
                    {
                        eolPos = 0;
                    }
                    else
                    {
                        eolPos = Splicer.GetPosOfEndOfLine(ref buffer);
                    }
                    var nextLine = Splicer.GetNextLine(ref buffer, ref eolPos, readDone);

                    // add index of text line
                    var entry = new Position();
                    entry.Timecode = Parser.Parse(nextLine);
                    entry.Offset = seekPos + eolPos;
                    _index.Add(entry);

                    // move to next seek position
                    seekPos += Interval;
                }
            }
        }

        public long GetPos(DateTime timecode)
        {
            var indexRange = FindRangeFromIndex(timecode);
            return GetPosFromFile(timecode, indexRange);
        }

        private Range FindRangeFromIndex(DateTime timecode)
        {
            if (_index == null)
                throw new Exception("no index found");

            var range = new Range();
            range.Start = _index.First();
            range.End = range.Start;

            if (timecode < range.Start.Timecode)
            {
                // searched timecode is before logfile
                range.End = range.Start;
                return range;
            }

            foreach (var p in _index)
            {
                if (range.Start.Timecode > timecode)
                    throw new Exception("range missed the correct start for range");
                if (p.Timecode > timecode)
                {
                    range.End = p;
                    return range;
                }
                // move range to next slot
                range.Start = range.End;
                range.End = p;
            }
            return range;
        }

        private long GetPosFromFile(DateTime timecode, Range preselection)
        {
            using(var fh = LogFile.OpenRead())
            {
                // create buffer for range
                var offsetDiff = preselection.GetOffsetDiff();
                if (offsetDiff > int.MaxValue)
                    throw new Exception("range is too big");
                int len = (int)(offsetDiff + bufLen);
                if (len < 1)
                    throw new Exception("not a valid buffer length");
                var buffer = new byte[len];

                // read range from file into buffer
                fh.Seek(preselection.Start.Offset, 0);
                var bytesRead = fh.Read(buffer, 0, len);
                if (bytesRead < len)
                    throw new Exception("only partial data read");

                // parse as test
                var logText = Encoding.UTF8.GetString(buffer, 0, len);
                throw new NotImplementedException();
            }
        }
    }
}
