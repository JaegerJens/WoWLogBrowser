using System;
namespace LogIndex
{
    public interface ITimecodeParser
    {
        DateTime Parse(string line);
    }
}
