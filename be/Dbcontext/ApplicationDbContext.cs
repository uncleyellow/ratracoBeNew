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
            // Thiết lập quan hệ giữa User và Department
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Users) // Một phòng ban có nhiều người dùng
                .WithOne(u => u.Department) // Một người dùng thuộc về một phòng ban
                .HasForeignKey(u => u.DepartmentId) // Khóa ngoại là DepartmentId trong User
                .OnDelete(DeleteBehavior.Cascade); // Xóa phòng ban sẽ xóa tất cả người dùng thuộc về

            // Thiết lập quan hệ giữa User và Mail
            modelBuilder.Entity<Mail>()
                .HasOne(m => m.FromUser) // Mỗi thư có một người gửi
                .WithMany(u => u.SentMails) // Một người dùng có nhiều thư đã gửi
                .HasForeignKey(m => m.FromUserId) // Khóa ngoại là FromUserId trong Mail
                .OnDelete(DeleteBehavior.Restrict); // Không xóa thư khi người gửi bị xóa
        }
    }
}
