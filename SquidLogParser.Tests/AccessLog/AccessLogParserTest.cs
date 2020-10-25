using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SquidLogParser.AccessLog;
using SquidLogParser.Tests.Fixtures;

namespace SquidLogParser.Tests.AccessLog
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
            var entries = _accessLogParser.ParseLogs(LogFileFixture.ReadLogs());

            entries.Should()
                .BeEquivalentTo(LogFileFixture.GetAccessEntries());
        }

        [Test]
        public void TestParseLog()
        {
            var log = LogFileFixture.ReadLogs()
                .First();

            var entry = _accessLogParser.ParseLog(log);

            var expectedEntry = LogFileFixture.GetAccessEntries()
                .First();

            entry.Should()
                .BeEquivalentTo(expectedEntry);
        }
    }
}