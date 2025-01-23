using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Avatar { get; set; } // Đường dẫn đến hình ảnh đại diện

        // Khóa ngoại đến Department
        public Guid? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Danh sách thư gửi
        public ICollection<Mail>? SentMails { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Avatar { get; set; } // Nếu cần thiết
    }
}