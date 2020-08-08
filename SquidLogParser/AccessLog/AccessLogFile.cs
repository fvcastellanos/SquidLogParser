using System;
using System.Collections.Generic;
using System.IO;
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
    }
}