using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace be.Models
{
    public class Mail
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Type { get; set; } // 'mail' hoặc 'internal'

        [Required]
        public Guid? FromUserId { get; set; } // ID người gửi

        [ForeignKey("FromUserId")]
        public User? FromUser { get; set; }

        [Required]
        public Guid? To { get; set; } // Người nhận

        public List<string> Cc { get; set; } = new List<string>(); // Danh sách CC

        public List<string> Bcc { get; set; } = new List<string>(); // Danh sách BCC

        [Required]
        public DateTime Date { get; set; } // Ngày gửi thư

        [Required]
        [StringLength(200)]
        public string? Subject { get; set; } // Chủ đề của thư

        [Required]
        public string? Content { get; set; } // Nội dung thư

        public List<Attachment> Attachments { get; set; } = new List<Attachment>(); // Danh sách tệp đính kèm

        public bool Starred { get; set; } // Đánh dấu thư yêu thích

        public bool Important { get; set; } // Đánh dấu thư quan trọng

        public bool Unread { get; set; } // Thư chưa đọc

        public Guid? FolderId { get; set; } // ID thư mục chứa thư này

        public List<string> Labels { get; set; } = new List<string>(); // Danh sách nhãn
    }

    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }
        public string? Type { get; set; } // Loại tệp (ví dụ: image/jpeg)
        public string? Name { get; set; } // Tên tệp
        public long? Size { get; set; } // Kích thước tệp (byte)
        public string? Preview { get; set; } // Đường dẫn để xem trước
        public string? DownloadUrl { get; set; } // Đường dẫn để tải xuống
    }


    public class MailDto
    {
        public string? Type { get; set; } // 'mail' hoặc 'internal'
        public Guid? FromUserId { get; set; } // ID người gửi
        public Guid? To { get; set; } // Danh sách người nhận
        public List<string>? Cc { get; set; } = new List<string>(); // Danh sách CC
        public List<string>? Bcc { get; set; } = new List<string>(); // Danh sách BCC
        public DateTime? Date { get; set; } // Ngày gửi thư
        public string? Subject { get; set; } // Chủ đề của thư
        public string? Content { get; set; } // Nội dung thư
        public List<Attachment>? Attachments { get; set; } = new List<Attachment>(); // Danh sách tệp đính kèm
        public bool? Starred { get; set; } // Đánh dấu thư yêu thích
        public bool? Important { get; set; } // Đánh dấu thư quan trọng
        public bool? Unread { get; set; } // Thư chưa đọc
        public Guid? FolderId { get; set; } // ID thư mục chứa thư này
        public List<string>? Labels { get; set; } = new List<string>(); // Danh sách nhãn
    }
}