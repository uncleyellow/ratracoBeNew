using be.Models;
using Microsoft.EntityFrameworkCore;

namespace be.Dbcontext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Mail> Mails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Departments)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Mails)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);
        }
    }
}
