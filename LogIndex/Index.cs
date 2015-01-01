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
                byte[] buffer = new byte[1024];
                while(true)
                {
                    // read buffer on seek position
                    fh.Seek(seekPos, 0);
                    var readDone = fh.Read(buffer, 0, 1024);
                    if (readDone == 0)
                    {
                        break;
                    }

                    // find and parse next text line
                    var eolPos = Splicer.GetPosOfEndOfLine(ref buffer);
                    var nextLine = Splicer.GetNextLine(ref buffer, ref eolPos, readDone);

                    // add index of text line
                    var entry = new Position();
                    entry.Timecode = Parser.Parse(nextLine);
                    entry.Offset = seekPos + eolPos + 1;
                    _index.Add(entry);

                    // move to next seek position
                    seekPos += Interval;
                }
            }
        }

        public long GetPos(DateTime timecode)
        {
            throw new NotImplementedException();
        }
    }
}
