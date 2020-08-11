using System;
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
        public void TestIgestFullLogs()
        {
            ExpectFullLogs();
            ExpectEmtpyDbSets();
            ExpectSaveLogEntries();
            ExpectSaveProcessHistory();

            _logService.IngestLogs();

            VerifyDbContext();
            VerifyFullLogs();
        }

        [Test]
        public void TestVerifyIngestFromIndex()
        {
            ExpectLogsUsingStartIndex();
            ExpectLogProcessHistories();
            ExpectSaveLogEntries();
            ExpectSaveProcessHistory();

            _logService.IngestLogs();

            VerifyDbContext();
            VerifyLogsUsingStartIndex();
        }

        // --------------------------------------------------------------------------------------------------

        private void ExpectFullLogs()
        {
            _accessLogMock.Setup(accessLog => accessLog.GetLogs())
                .Returns(LogFileFixture.ReadLogs());
        }

        private void ExpectLogsUsingStartIndex()
        {
            _accessLogMock.Setup(accessLog => accessLog.GetLogs(It.IsAny<long>()))
                .Returns(LogFileFixture.ReadLogsTrimmed());
        }

        private void ExpectEmtpyDbSets()
        {
            DbContextMock.Setup(context => context.AccessLogs)
                .ReturnsDbSet(new List<AccessLogEntry>());

            DbContextMock.Setup(context => context.LogProcessHistories)
                .ReturnsDbSet(new List<LogProcessHistory>());
        }

        private void ExpectLogProcessHistories()
        {
            DbContextMock.Setup(context => context.AccessLogs)
                .ReturnsDbSet(new List<AccessLogEntry>());

            DbContextMock.Setup(context => context.LogProcessHistories)
                .ReturnsDbSet(BuildLogProcessHistoryList());
        }

        private void ExpectSaveLogEntries()
        {
            DbContextMock.Setup(context => context.AccessLogs.AddRange(It.IsAny<IEnumerable<AccessLogEntry>>()));
            DbContextMock.Setup(context => context.SaveChanges());
        }

        private void ExpectSaveProcessHistory()
        {
            DbContextMock.Setup(context => context.LogProcessHistories.Add(It.IsAny<LogProcessHistory>()));
            DbContextMock.Setup(context => context.SaveChanges());
        }

        private void VerifyDbContext()
        {
            DbContextMock.Verify(context => context.AccessLogs.AddRange(It.IsAny<IEnumerable<AccessLogEntry>>()));
            DbContextMock.Verify(context => context.LogProcessHistories);
            DbContextMock.Verify(context => context.SaveChanges());            
        }

        private void VerifyFullLogs()
        {
            _accessLogMock.Setup(accessLog => accessLog.GetLogs());
        }

        private void VerifyLogsUsingStartIndex()
        {
            _accessLogMock.Setup(accessLog => accessLog.GetLogs(It.IsAny<long>()));
        }

        private IEnumerable<LogProcessHistory> BuildLogProcessHistoryList()
        {
            return new List<LogProcessHistory>()
            {
                new LogProcessHistory()
                {
                    Id = 0,
                    LinesProcessed = 2,
                    Processed = DateTime.Now
                }
            };
        }
    }    
}