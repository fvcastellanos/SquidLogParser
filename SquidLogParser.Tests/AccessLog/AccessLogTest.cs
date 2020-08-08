using FluentAssertions;
using NUnit.Framework;
using SquidLogParser.AccessLog;
using SquidLogParser.Tests.Common;
using SquidLogParser.Tests.Fixtures;

namespace SquidLogParser.Tests.AccessLog
{
    [TestFixture]
    public class AccessLogTest : BaseTest
    {
        private IAccessLog _accessLog;
        private string _logsFile;

        [SetUp]
        public void SetUp()
        {
            _logsFile = LogFileFixture.WriteLogs();
            _accessLog = new AccessLogFile(CreateLogger<AccessLogFile>(), _logsFile);
        }

        [TearDown]
        public void TearDown()
        {
            LogFileFixture.DeleteTempLogs(_logsFile);
        }

        [Test]
        public void TestGetLogs()
        {
            var logs = _accessLog.GetLogs();

            logs.Should()
                .BeEquivalentTo(LogFileFixture.ReadLogs());
        }
    }
}