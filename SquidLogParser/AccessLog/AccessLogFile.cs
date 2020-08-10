using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SquidLogParser.AccessLog
{
    public class AccessLogFile : IAccessLog
    {
        private readonly ILogger _logger;
        private readonly string _logPath;
        
        public AccessLogFile(ILogger<AccessLogFile> logger, string logPath)
        {
            _logger = logger;
            _logPath = logPath;
        }

        public string GetLogResource()
        {
            return _logPath;
        }

        public IEnumerable<string> GetLogs()
        {
            try
            {
                _logger.LogInformation("read log file: {0}", _logPath);
                return File.ReadLines(_logPath);
            }
            catch (Exception ex)
            {
                _logger.LogError("can't read log file: {0} - ", _logPath, ex);
                return new List<string>();
            }
        }

        public IEnumerable<string> GetLogs(long startIndex)
        {
            try
            {
                _logger.LogInformation("read log file: {0} from line: {1}", _logPath, startIndex);
                var lines = GetLogs();

                if (lines.Count() > startIndex)
                {
                    var array = lines.ToArray();
                    var subList = new List<string>();

                    for (long i=startIndex; i < lines.Count(); i++)
                    {
                        subList.Add(array[i]);
                    }

                    return subList;
                }

                _logger.LogInformation("no log entries found from start index: {0}", startIndex);
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't read log file: {0} from line: {1} - ", _logPath, startIndex, ex);
                return new List<string>();
            }
        }
    }
}