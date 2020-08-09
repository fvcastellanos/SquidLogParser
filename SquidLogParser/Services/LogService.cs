using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SquidLogParser.AccessLog;
using SquidLogParser.Data;
using SquidLogParser.Domain;

namespace SquidLogParser.Services
{
    public class LogService
    {
        private readonly ILogger _logger;
        private readonly IAccessLog _accessLog;
        private readonly IAccessLogParser _accessLogParser;
        private readonly SquidLogContext _dbContext;

        public LogService(ILogger<LogService> logger, 
                          IAccessLog accessLog,
                          IAccessLogParser accessLogParser,
                          SquidLogContext dbContext)
        {
            _logger = logger;
            _accessLog = accessLog;
            _accessLogParser = accessLogParser;
            _dbContext = dbContext;
        }

        public void IngestLogs()
        {
            try {
                _logger.LogInformation("getting logs from file");

                var textLogs = _accessLog.GetLogs();
                var entryLogs = _accessLogParser.ParseLogs(textLogs);

                _logger.LogInformation("storing logs into DB");
                
                var entities = BuildAccessLog(entryLogs);

                _dbContext.AccessLogs.AddRange(entities);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't process logs - ", ex);
            }
        }

        public IEnumerable<AccessLogEntry> GetFoo()
        {
            return _dbContext.AccessLogs;
        }

        // ------------------------------------------------------------------------------------------------

        private IEnumerable<AccessLogEntry> BuildAccessLog(IEnumerable<AccessEntry> accessEntryLogs)
        {
            return accessEntryLogs
                .Select(entry => {

                    return new AccessLogEntry()
                    {
                        Time = FromUnixTime(entry.Time),
                        Milliseconds = entry.Millis,
                        Duration = entry.Elapsed,
                        ClientAddress = entry.RemoteHost,
                        ResultCode = entry.Status,
                        Bytes = entry.Bytes,
                        RequestMethod = entry.Method,
                        Url = entry.Url,
                        User = entry.User,
                        Peer = entry.Peer,
                        Type = entry.Type
                    };

                }).ToList();
        }

        private DateTime FromUnixTime(long time)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(time);
            return dateTimeOffset.UtcDateTime;
        }
    }
}