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
            var mails = _context.Mails.ToList();
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

        [HttpPost]
        public ActionResult<Mail> PostMail(MailDto mailDto)
        {
            if (mailDto == null)
            {
                return BadRequest();
            }

            // Tạo đối tượng Mail từ MailDto
            var mail = new Mail
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                Type = mailDto.Type,
                FromUserId = mailDto.FromUserId,
                To = mailDto.To,
                Cc = mailDto.Cc,
                Bcc = mailDto.Bcc,
                Date = (DateTime)mailDto.Date,
                Subject = mailDto.Subject,
                Content = mailDto.Content,
                Attachments = mailDto.Attachments,
                Starred = (bool)mailDto.Starred,
                Important = (bool)mailDto.Important,
                Unread = (bool)mailDto.Unread,
                FolderId = mailDto.FolderId,
                Labels = mailDto.Labels
            };

            _context.Mails.Add(mail);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetMail), new { id = mail.Id }, mail);
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
    }
}