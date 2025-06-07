using Domain.Common.Ids;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EF.DbContexts
{
    public class UserDbContext : DbContext
    {
        // private string _connectionString = "Host=localhost;Port=5432;Database=dot-net-starter;Username=admin;Password=Admin321";

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => EntityId.Create(v))
                    .HasDefaultValueSql("gen_random_uuid()");
                
                entity.Property(e => e.RoleId)
                    .HasConversion(
                        v => v.Value,
                        v => EntityId.Create(v))
                    .HasDefaultValueSql("gen_random_uuid()");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => EntityId.Create(v))
                    .HasDefaultValueSql("gen_random_uuid()");
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseNpgsql(_connectionString, b => b.MigrationsAssembly("Persistence.EF"));
        // }
    }
}
