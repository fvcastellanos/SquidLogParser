using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;
using SquidLogParser.Domain;

namespace SquidLogParser.Services
{
    public class AccessLogParser : IAccessLogParser
    {
        private readonly ILogger _logger;

        public AccessLogParser(ILogger<AccessLogParser> logger)
        {
            _logger = logger;
        }

        public void ParseFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                var logEntires = File.ReadLines(fileName);

                var logEntryList = logEntires.Select(parseLine)
                    .ToList();
            }

        }

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
                PeerHost = fields[7],
                Type = fields[8]
            };
        }
    }
}