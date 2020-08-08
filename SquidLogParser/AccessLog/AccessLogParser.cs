using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;
using SquidLogParser.Domain;
using System.Collections.Generic;

namespace SquidLogParser.AccessLog
{
    public class AccessLogParser : IAccessLogParser
    {
        private readonly ILogger _logger;

        public AccessLogParser(ILogger<AccessLogParser> logger)
        {
            _logger = logger;
        }

        public IEnumerable<AccessEntry> ParseLogs(IEnumerable<string> logs)
        {
            return logs.Select(parseLine)
                .ToList();
        }

        // ------------------------------------------------------------------------------------

        private AccessEntry parseLine(string entry)
        {
            var fields = entry.Split(" ").ToList()
                .Where(text => !string.IsNullOrEmpty(text))
                .ToArray();

            return new AccessEntry()
            {
                Time = double.Parse(fields[0]),
                Elapsed = double.Parse(fields[1]),
                RemoteHost = fields[2],
                Status = fields[3],
                Bytes = long.Parse(fields[4]),
                Method = fields[5],
                Url = fields[6],
                User = fields[7],
                Peer = fields[8],
                Type = fields[9]
            };
        }
    }
}