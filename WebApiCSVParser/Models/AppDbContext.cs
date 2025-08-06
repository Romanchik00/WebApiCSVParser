using Microsoft.EntityFrameworkCore;

namespace WebApiCSVParser.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Value> Values => Set<Value>();
        public DbSet<Result> Results => Set<Result>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Result>()
                .HasIndex(r => r.FileName)
                .IsUnique();
        }
    }
}
