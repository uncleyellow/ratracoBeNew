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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Lấy danh sách người dùng
       [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var users = _context.Users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            }).ToList();
            return Ok(users);
        }

        // Lấy thông tin người dùng theo ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //Thêm người dùng mới
       [HttpPost]
        public ActionResult<User> PostUser(UserDto userCreateDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                Name = userCreateDto.Name,
                Email = userCreateDto.Email,
                Password = userCreateDto.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Cập nhật thông tin người dùng theo ID
        [HttpPut("{id}")]
        public ActionResult<User> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Xóa người dùng theo ID
        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}