using be.Dbcontext;
using be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _context.Users.Select(user => new UserCreateDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }).ToList();
            return Ok(users);
        }

        [HttpPost]
        public ActionResult<User> PostUser(UserCreateDto userCreateDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                Password = userCreateDto.Password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

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