using System.Collections.Generic;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SquidLogParser.AccessLog;
using SquidLogParser.Data;
using SquidLogParser.Services;
using SquidLogParser.Tests.Fixtures;

namespace SquidLogParser.Tests.Services
{
    [TestFixture]
    public class LogServiceTest : ServiceTestBase
    {
        private Mock<IAccessLog> _accessLogMock = new Mock<IAccessLog>();

        private LogService _logService;

        [SetUp]
        public void SetUp()
        {
            var accessLogParser = new AccessLogParser(CreateLogger<AccessLogParser>());
            _logService = new LogService(CreateLogger<LogService>(), 
                _accessLogMock.Object, accessLogParser, DbContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            DbContextMock.Reset();
            _accessLogMock.Reset();
        }

        [Test]
        public void IgestLogsTest()
        {
            _accessLogMock.Setup(accessLog => accessLog.GetLogs())
                .Returns(LogFileFixture.ReadLogs());

            DbContextMock.Setup(context => context.AccessLogs)
                .ReturnsDbSet(new List<AccessLogEntry>());

            DbContextMock.Setup(context => context.AccessLogs.AddRange(It.IsAny<IEnumerable<AccessLogEntry>>()));
            DbContextMock.Setup(context => context.SaveChanges());

            _logService.IngestLogs();

            DbContextMock.Verify(context => context.AccessLogs.AddRange(It.IsAny<IEnumerable<AccessLogEntry>>()));
            DbContextMock.Verify(context => context.SaveChanges());
        }
    }    
}