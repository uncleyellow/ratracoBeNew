﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be.Models
{
    public class Mail
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public required string EmailAddress { get; set; }

        // Khóa ngoại đến User
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
