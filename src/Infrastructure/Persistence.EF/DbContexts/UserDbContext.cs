using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EF.DbContexts
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(r => r.Name).HasMaxLength(100);
        }
    }
}
