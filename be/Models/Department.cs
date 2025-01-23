using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace be.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public bool InternalMail { get; set; } // Xác định liệu phòng ban có sử dụng thư nội bộ hay không

        // Danh sách người dùng thuộc phòng ban
        public ICollection<User>? Users { get; set; }
    }
}