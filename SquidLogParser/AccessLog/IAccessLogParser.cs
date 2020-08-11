using System.Collections.Generic;
using SquidLogParser.Domain;

namespace SquidLogParser.AccessLog
{
    public interface IAccessLogParser
    {
        IEnumerable<AccessEntry> ParseLogs(IEnumerable<string> logs);
    }
}