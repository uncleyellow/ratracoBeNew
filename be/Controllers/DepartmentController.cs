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
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả phòng ban
        [HttpGet]
        public ActionResult<IEnumerable<Department>> GetDepartments()
        {
            var departments = _context.Departments.ToList();
            return Ok(departments);
        }

        // Lấy thông tin phòng ban theo ID
        [HttpGet("{id}")]
        public ActionResult<Department> GetDepartment(Guid id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        // Lấy danh sách người dùng trong phòng ban
        [HttpGet("{id}/users")]
        public ActionResult<IEnumerable<User>> GetUsersInDepartment(Guid id)
        {
            var department = _context.Departments.Include(d => d.Users).FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            var users = department?.Users?.ToList();
            return Ok(users);
        }

        // Thêm phòng ban mới
        [HttpPost]
        public ActionResult<Department> PostDepartment(Department department)
        {
            if (department == null)
            {
                return BadRequest();
            }

            department.Id = Guid.NewGuid(); // Tạo ID mới
            _context.Departments.Add(department);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department);
        }

        // Cập nhật thông tin phòng ban theo ID
        [HttpPut("{id}")]
        public ActionResult PutDepartment(Guid id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // Xóa phòng ban theo ID
        [HttpDelete("{id}")]
        public ActionResult DeleteDepartment(Guid id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();

            return NoContent();
        }
    }
}