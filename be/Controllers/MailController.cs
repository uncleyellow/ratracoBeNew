using be.Dbcontext;
using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // Lấy danh sách tất cả thư
        [HttpGet]
        public ActionResult<IEnumerable<Mail>> GetMails()
        {
            var mails = _context.Mails?.ToList();
            return Ok(mails);
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
            var mails = _context.Mails.Where(m => m.FromUserId == userId || m.To == userId).ToList();
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
                Type = "mail",
                FromUserId = mailDto.FromUserId,
                To = mailDto.To ?? new Guid(),
                Cc = mailDto.Cc,
                Bcc = mailDto.Bcc,
                Subject = mailDto.Subject,
                Content = mailDto.Content,
                Date = DateTime.UtcNow, // Ghi lại thời gian gửi
                Starred = false, // Mặc định
                Important = false, // Mặc định
                Unread = true, // Mặc định
                Attachments = new List<Attachment>(), // Mặc định
                Labels = new List<string>(), // Mặc định
                FolderId = new Guid() // Mặc định
            };

            _context.Mails.Add(mail);
            _context.SaveChanges();

            return mail;
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

        [HttpGet("api/users/{userId}/mails")]
        public ActionResult<List<Mail>> GetMailsByUserId(Guid userId)
        {
            var mails = _context.Mails
                .Where(mail => mail.FromUserId == userId ||
                               mail.To == userId ||
                               (mail.Cc != null && mail.Cc == userId) ||
                               (mail.Bcc != null && mail.Bcc == userId))
                .ToList();

            if (!mails.Any())
            {
                return NotFound();
            }

            return Ok(mails);
        }

    }


}