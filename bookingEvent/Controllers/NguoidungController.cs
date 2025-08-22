using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguoiDungController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NguoiDungController> _logger;

        public NguoiDungController(ApplicationDbContext context, ILogger<NguoiDungController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<NguoiDung>>> GetAll()
        {
            return await _context.NguoiDung.ToListAsync();
        }

        // Lấy 1 người dùng theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<NguoiDung>> GetById(Guid id)
        {
            var user = await _context.NguoiDung.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // Thêm mới 1 người dùng
        [HttpPost]
        public async Task<ActionResult<NguoiDung>> Create(NguoiDung user)
        {
            user.ma = Guid.NewGuid();
            _context.NguoiDung.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.ma }, user);
        }

        // Cập nhật 1 người dùng
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, NguoiDung user)
        {
            if (id != user.ma) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Xoá 1 người dùng
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.NguoiDung.FindAsync(id);
            if (user == null) return NotFound();

            _context.NguoiDung.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
