using Microsoft.EntityFrameworkCore;
using IgrejaDashboard.Api.Models;

// Aqui'é o "elo" entre o código e o banco de dados real, o DbSet vira uma tabela Pessoas no SQL Server

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
