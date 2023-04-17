using Builders.Bills.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Builders.Bills.Database
{
    public class BillsDbContext : DbContext
    {
        public BillsDbContext()
        {
        }

        public BillsDbContext(DbContextOptions<BillsDbContext> options)
      : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
                optionsBuilder.UseSqlite($@"Data Source=:memory:;");
        }

        public virtual DbSet<Bill> Bills { get; set; }
    }
}