using Microsoft.Extensions.Logging;
using System.Linq;
using SquidLogParser.Domain;
using System.Collections.Generic;
using System;

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
            return logs.Filter(entry => !string.IsNullOrEmpty(entry))
                .Select(parseLine)
                .ToList();
        }

        // ------------------------------------------------------------------------------------

        private AccessEntry parseLine(string entry)
        {
            try
            {
                var fields = entry.Split(" ").ToList()
                    .Where(text => !string.IsNullOrEmpty(text))
                    .ToArray();

                var time = fields[0].Substring(0, 10);
                var separatorPosition = fields[0].LastIndexOf(".");
                var millis = fields[0].Substring(separatorPosition + 1);

                return new AccessEntry()
                {
                    Time = long.Parse(time),
                    Elapsed = int.Parse(millis),
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
            catch (Exception ex)
            {
                _logger.LogError("can't parse log entry: {0} - ", entry, ex.Message);
                return new AccessEntry();
            }
        }
    }
}