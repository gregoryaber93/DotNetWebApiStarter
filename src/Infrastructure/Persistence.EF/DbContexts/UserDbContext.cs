using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EF.DbContexts
{
    public class UserDbContext : DbContext
    {
        private string _connectionString = "Host=localhost;Port=5432;Database=dot-net-starter;Username=admin;Password=Admin321";
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(r => r.Name).HasMaxLength(100);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, b => b.MigrationsAssembly("Persistence.EF"));
        }
    }
}
