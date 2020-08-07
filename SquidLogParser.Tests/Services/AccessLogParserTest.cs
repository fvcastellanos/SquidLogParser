using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SquidLogParser.Services;

namespace SquidLogParser.Tests.Services
{
    [TestFixture]
    public class AccessLogParserTest
    {
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory();
        private IAccessLogParser _accessLogParser;

        [SetUp]
        public void SetUp()
        {
            _accessLogParser = new AccessLogParser(LoggerFactory.CreateLogger<AccessLogParser>());
        }

        [Test]
        public void TestParseFile()
        {
            _accessLogParser.ParseFile(@"C:\Users\fvcg\Desktop\access.log");
        }
    }
}