using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
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

        public Either<string, long> IngestLogs()
        {
            try {

                _logger.LogInformation("looking for previous processed logs");
                var linesProcessed = GetLinesProcessed();

                _logger.LogInformation("the following logs have been already processed: {0}", linesProcessed);

                _logger.LogInformation("getting logs from file");

                var textLogs = GetLogs(linesProcessed);
                var entryLogs = _accessLogParser.ParseLogs(textLogs);

                _logger.LogInformation("storing logs into DB");
                
                var entities = BuildAccessLog(entryLogs);

                _dbContext.AccessLogs.AddRange(entities);
                _dbContext.SaveChanges();

                var processedLogs = entryLogs.Count();
                _logger.LogInformation("logs processed: {0}", processedLogs);

                return processedLogs;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't process logs - ", ex);
                return string.Format("Can't process log resource: {0}", _accessLog.GetLogResource());
            }
        }

        public Either<string, AccessEntry> ProcessLogEntry(string log)
        {
            try 
            {
                _logger.LogInformation("Log entry received: {0}", log);

                var entry = _accessLogParser.ParseLog(log);

                var accessLog = BuildAccessLogEntry(entry);

                _dbContext.AccessLogs.Add(accessLog);
                _dbContext.SaveChanges();

                _logger.LogInformation("Entry log: {0} has been stored in db", log);

                return entry;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't process log: {0} - ", log, ex);
                return String.Format("Can't process log entry: {0}", log);
            }
        }

        // ------------------------------------------------------------------------------------------------

        private IEnumerable<string> GetLogs(long linesProcessed)
        {
            long processedLogs = 0;
            IEnumerable<string> textLogs;
            if (linesProcessed == 0)
            {
                textLogs = _accessLog.GetLogs();
                processedLogs = textLogs.Count();
            }
            else {
                textLogs = _accessLog.GetLogs(linesProcessed);
                processedLogs = linesProcessed + textLogs.Count();
            }

            SaveProcessedLog(processedLogs);

            return textLogs;
        }

        private IEnumerable<AccessLogEntry> BuildAccessLog(IEnumerable<AccessEntry> accessEntryLogs)
        {
            return accessEntryLogs
                .Filter(entry => entry.Time > 0)
                .Select(BuildAccessLogEntry)
                .ToList();
        }

        public static AccessLogEntry BuildAccessLogEntry(AccessEntry entry)
        {
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
        }

        private static DateTime FromUnixTime(long time)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
            return dateTimeOffset.UtcDateTime;
        }

        private long GetLinesProcessed()
        {
            return  _dbContext.LogProcessHistories
                .OrderByDescending(history => history.Id)
                .Select(history => history.LinesProcessed)
                .FirstOrDefault();
        }

        private void SaveProcessedLog(long lastLine)
        {
            var history = new LogProcessHistory()
            {
                LinesProcessed = lastLine
            };

            _dbContext.LogProcessHistories.Add(history);
            _dbContext.SaveChanges();
        }
    }
}