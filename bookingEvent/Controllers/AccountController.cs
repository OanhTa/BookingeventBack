using bookingEvent.Data;
using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Account>>> View()
        {
            return await _context.Account.ToListAsync();
        }

        // Lấy 1 người dùng theo id
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(Guid id)
        {
            var user = await _context.Account.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // Thêm mới 1 người dùng
        [HttpPost]
        public async Task<ActionResult<Account>> Add(Account user)
        {
            user.Id = Guid.NewGuid();
            _context.Account.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // Cập nhật 1 người dùng
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, Account user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Xoá 1 người dùng
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Account.FindAsync(id);
            if (user == null) return NotFound();

            _context.Account.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
