using Microsoft.EntityFrameworkCore;
using Tutorial.DataAccess.MSSQL.EntityConfigurations;
using Tutorial.Domain.Entities;

namespace Tutorial.DataAccess.MSSQL.Context
{
    public class TutorialDbContext : DbContext
    {
        public TutorialDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Server> Servers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.ApplyConfiguration(new ServerEntityConfiguration());
        }
    }
}
