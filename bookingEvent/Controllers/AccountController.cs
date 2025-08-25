using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Services;
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
        private readonly AccountService _service;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountService service, ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _service = service;
           _logger = logger;
            
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Account>>> View()
        {
            return await _context.Account.ToListAsync();
        }


        // Lấy 1 người dùng theo id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetById(Guid id)
        {
            var user = await _context.Account.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPost("update-token")]
        public async Task<IActionResult> EditToken([FromBody] UpdateTokenRequest request)
        {
            if (request == null || request.AccountId == Guid.Empty || String.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            var result = await _service.UpdateToken(request.AccountId, request.Token);
            if (!result)
                return StatusCode(500, "Có lỗi khi cập nhật token");

            var accountDto = await _service.GetAccountDtoAsync(request.AccountId);
            if (accountDto == null)
                return NotFound("Không tìm thấy tài khoản");

            return Ok(new
            {
                message = "Cập nhật token thành công",
                account = accountDto
            });
        }

        // Thêm mới 1 người dùng
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Account>> Add(Account user)
        {
            user.Id = Guid.NewGuid();
            _context.Account.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // Cập nhật 1 người dùng
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id, Account user)
        {
            if (id != user.Id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Xoá 1 người dùng
        [HttpDelete("{id}")]
        [Authorize]
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
public class UpdateTokenRequest
{
    public Guid AccountId { get; set; }
    public string Token { get; set; }
}
