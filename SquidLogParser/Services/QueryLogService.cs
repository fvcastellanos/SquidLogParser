using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
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

        public Either<string, IEnumerable<AccessLogView>> GetTopVisitedSitesLastNDays(int top = 10, int days = 30)
        {
            try
            {
                var queryResult = from accessLog in _dbContext.AccessLogs
                    where accessLog.Time >= LastNDays(days)
                    group accessLog by new { 
                        accessLog.Url, 
                        accessLog.Time.Date, 
                        accessLog.ClientAddress,
                        accessLog.RequestMethod
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
                        Time = result.Key.Date,
                        ClientAddress = result.Key.ClientAddress,
                        RequestMethod = result.Key.RequestMethod,
                        Count = result.Count                        
                    }).Take(top)
                    .ToList();                    
            }
            catch(Exception ex)
            {
                _logger.LogError("can't get most visited sites - {0}", ex);
                return string.Format("Can't get top: {0} visited sites", top);
            }
        }

        public Either<string, IEnumerable<AccessLogView>> GetTopSitesByUser(string user, int top = 10, int days = 30)
        {
            try 
            {
                var queryResult = from accessLog in _dbContext.AccessLogs
                    where accessLog.ClientAddress.Equals(user)
                        && accessLog.Time >= LastNDays(days)
                    group accessLog by new { 
                        accessLog.Url, 
                        accessLog.Time.Date, 
                        accessLog.RequestMethod,
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
                        Time = result.Key.Date,
                        RequestMethod = result.Key.RequestMethod,
                        Count = result.Count                        
                    }).Take(top)
                    .ToList();                    
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get most visited sites for user {0} - ", user, ex);
                return string.Format("Can't get top: {0} visited sites for user {1}", top, user);
            }
        }

        public Either<string, IEnumerable<AccessLogView>> Foo(int month, int year, string user, int top = 10)
        {
            try
            {
                return new List<AccessLogView>();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get most visited sites for user {0} - ", user, ex);
                return string.Format("Can't get top: {0} visited sites for user {1}", top, user);
            }
        }

        public Either<string, IEnumerable<string>> GetUsers()
        {
            try
            {
                return _dbContext.AccessLogs
                    .Select(accessLog => accessLog.ClientAddress)
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get users - ", ex);
                return "Can't get users";
            }
        }

        private DateTime LastNDays(int days)
        {
            return DateTime.Today.AddDays(days * -1);
        }
    }
}