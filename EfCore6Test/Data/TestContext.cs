using EfCore6Test.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore6Test.Data
{
    public class TestContext : DbContext
    {

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Role>().ToTable("role");

            modelBuilder.Entity<Role>().HasIndex(r => r.RoleTitle).IsUnique();
        }
    }
}
