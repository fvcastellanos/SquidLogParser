using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LanguageExt.UnitTesting;
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

            var result = _logService.IngestLogs();

            result.ShouldBeRight();

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

            var result = _logService.IngestLogs();

            result.ShouldBeRight();

            VerifyDbContext();
            VerifyLogsUsingStartIndex();
        }

        [Test]
        public void TestIngestThrowsException()
        {
            ExpectFullLogs();
            ExpectEmtpyDbSets();

            DbContextMock.Setup(context => context.AccessLogs
                .AddRange(It.IsAny<IEnumerable<AccessLogEntry>>()));

            DbContextMock.Setup(context => context.SaveChanges())
                .Throws(new Exception("test exception"));

            var result = _logService.IngestLogs();

            result.ShouldBeLeft();
            
            VerifyFullLogs();
        }

        [Test]
        public void TestProcessEntryLog()
        {
            var entry = LogFileFixture.ReadLogs()
                .First();

            ExpectFullLogs();
            ExpectSaveLogEntry();

            _logService.ProcessLogEntry(entry);

            VerifyFullLogs();
            VerifyLogEntrySaved();
        }

        [Test]
        public void TestProcessEntryLogThrowsException()
        {
            var entry = LogFileFixture.ReadLogs()
                .First();

            ExpectFullLogs();

            DbContextMock.Setup(context => context.AccessLogs
                .Add(It.IsAny<AccessLogEntry>()));

            DbContextMock.Setup(context => context.SaveChanges())
                .Throws(new Exception("test exception"));

            var result = _logService.ProcessLogEntry(entry);

            result.ShouldBeLeft();

            VerifyFullLogs();
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

        private void ExpectSaveLogEntry()
        {
            DbContextMock.Setup(context => context.AccessLogs.Add(It.IsAny<AccessLogEntry>()));
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

        private void VerifyLogEntrySaved()
        {
            DbContextMock.Setup(context => context.AccessLogs.Add(It.IsAny<AccessLogEntry>()));
            DbContextMock.Setup(context => context.SaveChanges());
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