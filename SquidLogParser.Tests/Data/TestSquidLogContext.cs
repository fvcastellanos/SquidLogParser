using Microsoft.EntityFrameworkCore;
using SquidLogParser.Data;

namespace SquidLogParser.Tests.Data
{
    public class TestSquidLogContext : SquidLogContext
    {
        public TestSquidLogContext() : base(new DbContextOptions<SquidLogContext>())
        {
            // test db context used for mocks
        }
    }
}