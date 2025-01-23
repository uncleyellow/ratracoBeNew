using be.Dbcontext;
using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/mails")]
        public ActionResult GetMails()
        {
            try
            {
                var mails = _context.Mails
                    .Select(m => new
                    {
                        m.Id,
                        m.Type,
                        m.FromUserId,
                        m.To,
                        m.Cc,
                        m.Bcc,
                        m.Date,
                        m.Subject,
                        m.Content,
                        m.Starred,
                        m.Important,
                        m.Unread
                    })
                    .ToList();

                return Ok(mails);
            }
            catch (Exception ex)
            {
                // Log the full exception 
                return BadRequest($"Error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // Lấy thông tin thư theo ID
        [HttpGet("{id}")]
        public ActionResult<Mail> GetMail(Guid id)
        {
            var mail = _context.Mails.Find(id);
            if (mail == null)
            {
                return NotFound();
            }
            return Ok(mail);
        }

        // Lấy danh sách thư theo người dùng
        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Mail>> GetMailsByUser(Guid userId)
        {
            var mails = _context.Mails
                .Where(m => m.FromUserId == userId ||
                             m.To.Contains(userId) ||
                             m.Cc.Contains(userId) ||
                             m.Bcc.Contains(userId))
                .ToList();

            if (mails == null || !mails.Any())
            {
                return NotFound();
            }

            return Ok(mails);
        }
        // Thêm thư mới
        [HttpPost]
        public ActionResult<Mail> PostMail(Mail mail)
        {
            if (mail == null)
            {
                return BadRequest();
            }

            mail.Id = Guid.NewGuid(); // Tạo ID mới
            _context.Mails.Add(mail);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMail), new { id = mail.Id }, mail);
        }

        [HttpPost("api/mails")]
        public ActionResult<Mail> PostMail(MailPostDto mailDto)
        {
            if (mailDto == null)
            {
                return BadRequest();
            }

            var mail = new Mail
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                Type = mailDto.Type, // Gán loại từ mailDto
                FromUserId = mailDto.FromUserId,
                To = mailDto.To ?? new List<Guid>(), // Gán danh sách người nhận, mặc định là danh sách rỗng
                Cc = mailDto.Cc ?? new List<Guid>(), // Gán danh sách CC, mặc định là danh sách rỗng
                Bcc = mailDto.Bcc ?? new List<Guid>(), // Gán danh sách BCC, mặc định là danh sách rỗng
                Subject = mailDto.Subject,
                Content = mailDto.Content,
                Date = DateTime.UtcNow, // Ghi lại thời gian gửi
                Starred = false, // Mặc định
                Important = false, // Mặc định
                Unread = true, // Mặc định
                Attachments = new List<Attachment>(), // Mặc định
                Labels = new List<string>(), // Mặc định
                FolderId = null // Để null nếu không có giá trị
            };

            _context.Mails.Add(mail);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMailById), new { id = mail.Id }, mail); // Trả về mail mới tạo
        }

        // Cập nhật thông tin thư theo ID
        [HttpPut("{id}")]
        public ActionResult PutMail(Guid id, Mail mail)
        {
            if (id != mail.Id)
            {
                return BadRequest();
            }

            _context.Entry(mail).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Mails.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // Xóa thư theo ID
        [HttpDelete("{id}")]
        public ActionResult DeleteMail(Guid id)
        {
            var mail = _context.Mails.Find(id);
            if (mail == null)
            {
                return NotFound();
            }

            _context.Mails.Remove(mail);
            _context.SaveChanges();

            return NoContent();
        }



        // Lấy thông tin chi tiết của một email theo ID
        [HttpGet("mails/{id}")]
        public ActionResult<Mail> GetMailById(Guid id)
        {
            var mail = _context.Mails.Find(id);

            if (mail == null)
            {
                return NotFound();
            }

            return Ok(mail);
        }

    }


}