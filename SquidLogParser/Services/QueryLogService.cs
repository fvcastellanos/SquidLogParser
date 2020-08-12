using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SquidLogParser.Data;
using SquidLogParser.Domain;

namespace SquidLogParser.Services
{
    public class QueryLogService
    {
        private readonly ILogger _logger;
        private readonly SquidLogContext _dbContext;

        public QueryLogService(ILogger<QueryLogService> logger, SquidLogContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Either<string, IEnumerable<AccessLogView>> GetTopVisitedSites(int top = 10)
        {
            try
            {
                var queryResult = from accessLog in _dbContext.AccessLogs
                    group accessLog by new { 
                        accessLog.Url, 
                        accessLog.Time, 
                        accessLog.Duration, 
                        accessLog.ClientAddress,
                        accessLog.Bytes,
                        accessLog.RequestMethod,
                        accessLog.Peer,
                        accessLog.Type,
                        accessLog.ResultCode
                    } into groupedLogs
                    orderby groupedLogs.Count() descending
                    select new {
                        groupedLogs.Key,
                        Count = groupedLogs.Count()
                    };

                return queryResult
                    .Select(result => new AccessLogView()
                    {
                        Url = result.Key.Url,
                        Time = result.Key.Time,
                        Duration = result.Key.Duration,
                        ClientAddress = result.Key.ClientAddress,
                        Bytes = result.Key.Bytes,
                        RequestMethod = result.Key.RequestMethod,
                        Peer = result.Key.Peer,
                        Type = result.Key.Type,
                        ResultCode = result.Key.ResultCode,
                        Count = result.Count                        
                    }).Take(top)
                    .ToList();                    
            }
            catch(Exception ex)
            {
                _logger.LogError("can't get most visited sites - ", ex);
                return string.Format("Can't get top: {0} visited sites", top);
            }
        }
    }
}