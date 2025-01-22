using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace be.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public  string Password { get; set; }

        // Điều này cho phép mỗi người dùng có nhiều department
        public ICollection<Department>? Departments { get; set; } = new List<Department>();

        // Điều này cho phép mỗi người dùng có nhiều mail
        public ICollection<Mail>? Mails { get; set; } = new List<Mail>();
    }

    public class UserCreateDto
    {

        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
