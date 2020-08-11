using System;
using Moq;
using SquidLogParser.Tests.Common;
using SquidLogParser.Tests.Data;

namespace SquidLogParser.Services
{
    public abstract class ServiceTestBase : BaseTest
    {
        protected readonly Mock<TestSquidLogContext> DbContextMock = new Mock<TestSquidLogContext>();

        protected static Exception TestException()
        {
            return new Exception("test exception");
        }        
    }
}