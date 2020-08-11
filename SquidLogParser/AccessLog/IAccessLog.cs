using System.Collections.Generic;
using System.Threading.Tasks;

namespace SquidLogParser.AccessLog
{
    public interface IAccessLog
    {
        IEnumerable<string> GetLogs();
        IEnumerable<string> GetLogs(long startIndex);
        string GetLogResource();
    }
}