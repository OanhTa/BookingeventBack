using bookingEvent.Data;
using bookingEvent.Model;
using bookingEvent.Services.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public AuthController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var account = _context.Account.FirstOrDefault(u => u.Email == request.Email);
                if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PassHash))
                {
                    return Unauthorized(new { message = "Sai email hoặc mật khẩu" });
                }

                var token = _authService.GenerateToken(account);
                return Ok(new
                {
                    token,
                    accountId = account.Id
                });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, new { message = "Lỗi server", detail = ex.Message, innerDetail = inner });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var existing = _context.Account.FirstOrDefault(a => a.Email == request.Email);
            if (existing != null)
                return BadRequest(new { message = "Email đã được sử dụng" });
            var defaultGroup = _context.AccountGroup.FirstOrDefault(g => g.Name == "User");
            if (defaultGroup == null)
                return StatusCode(500, new { message = "Không tìm thấy nhóm User trong hệ thống" });

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                AccountGroupId = defaultGroup.Id,
                Phone = request.Phone,
                PassHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };
            try
            {
                _context.Account.Add(account);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : "";
                return StatusCode(500, new { message = "Lỗi server", detail = ex.Message, innerDetail = inner });
            }

            var token = _authService.GenerateToken(account);
            return Ok(new { token, accountId = account.Id });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}
