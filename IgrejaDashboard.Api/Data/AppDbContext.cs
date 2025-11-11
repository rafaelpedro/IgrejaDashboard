using Microsoft.EntityFrameworkCore;
using IgrejaDashboard.Api.Models;

namespace IgrejaDashboard.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .Property(p => p.Status)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
