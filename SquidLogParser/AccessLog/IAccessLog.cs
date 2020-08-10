using System.Collections.Generic;

namespace SquidLogParser.AccessLog
{
    public interface IAccessLog
    {
        IEnumerable<string> GetLogs();
        IEnumerable<string> GetLogs(long startIndex);
        string GetLogResource();
    }
}