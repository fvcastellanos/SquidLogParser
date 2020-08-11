using Microsoft.Extensions.Logging;

namespace SquidLogParser.Tests.Common
{
    public abstract class BaseTest
    {
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory();

        protected ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory.CreateLogger<T>();
        }
    }
}