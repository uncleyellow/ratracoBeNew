using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        // Khóa ngoại đến User
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
