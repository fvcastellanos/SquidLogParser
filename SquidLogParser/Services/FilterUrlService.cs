using System;
using System.Linq;
using System.Collections.Generic;
using LanguageExt;
using Microsoft.Extensions.Logging;
using SquidLogParser.Data;
using SquidLogParser.Domain;

namespace SquidLogParser.Services
{
    public class FilterUrlService
    {
        private readonly ILogger _logger;

        private readonly SquidLogContext _dbContext;

        public FilterUrlService(ILogger<FilterUrlService> logger, SquidLogContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Either<string, long> AddUrl(string url)
        {
            try
            {
                var existingUrlHolder = _dbContext.FilteredUrls.Find(filteredUrl => filteredUrl.Url.Equals(url));

                if (existingUrlHolder.IsSome)
                {
                    _logger.LogInformation("URL: {0} already exists in filtered url set", url);
                    return string.Format("URL: {0} already exists in filtered url set", url);
                }

                _logger.LogInformation("Adding URL: {0} to filtered sites", url);
                var filteredUrl = new FilteredUrl()
                {
                    Url = url
                };

                _dbContext.FilteredUrls.Add(filteredUrl);
                _dbContext.SaveChanges();

                return filteredUrl.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't add new URL: {0} to be filtered - {1}", url, ex.Message);
                return string.Format("Can't add URL: {0} to filtered sites", url);
            }
        }

        public Either<string, int> RemoveUrl(string url)
        {
            try
            {
                var existingUrlHolder = _dbContext.FilteredUrls.Find(filteredUrl => filteredUrl.Url.Equals(url));

                int rows = 0;
                existingUrlHolder.Match(Some => {
                    _dbContext.FilteredUrls.Remove(Some);
                    _dbContext.SaveChanges();
                    rows = 1;
                }, () => {
                    _logger.LogInformation("URL: {0} not found", url);
                });

                return rows;
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't delete URL: {0} - {1}", url, ex.Message);
                return string.Format("Can't delete URL: {0}", url);
            }
        }

        public Either<string, List<FilteredUrlView>> GetUrls(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    return _dbContext.FilteredUrls
                        .Select(BuildFilteredUrlView)
                        .ToList();
                }

                return _dbContext.FilteredUrls
                    .Where(filteredUrl => filteredUrl.Url.Contains(url))
                    .Select(BuildFilteredUrlView)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Can't get URL list - {0}", ex.Message);
                return "Can't get URL list";
            }
        }

        // ------------------------------------------------------------------------------------------

        private FilteredUrlView BuildFilteredUrlView(FilteredUrl filteredUrl)
        {
            return new FilteredUrlView()
            {
                Id = filteredUrl.Id,
                Url = filteredUrl.Url
            };
        }
    }
}