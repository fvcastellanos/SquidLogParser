using Microsoft.EntityFrameworkCore;

namespace SquidLogParser.Data
{
    public class SquidLogContext : DbContext
    {
        public virtual DbSet<AccessLog> AccessLogs { get; set; }

        public SquidLogContext(DbContextOptions<SquidLogContext> dbContext) : base(dbContext)
        {
            // db context initialize
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("squid_log");

            // AccessLog

            modelBuilder.Entity<AccessLog>()
                .HasIndex(p => p.Time)
                .HasName("idx_access_log_time");

            modelBuilder.Entity<AccessLog>()
                .HasIndex(p => p.Url)
                .HasName("idx_access_log_url");

            modelBuilder.Entity<AccessLog>()
                .HasIndex(p => p.ClientAddress)
                .HasName("idx_access_log_client_address");

            modelBuilder.Entity<AccessLog>()
                .HasIndex(p => p.Bytes)
                .HasName("idx_access_bytes");
        }
    }
}