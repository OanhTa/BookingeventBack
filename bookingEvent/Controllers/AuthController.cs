using bookingEvent.Data;
using bookingEvent.Model;
using bookingEvent.Services.Auth;
using Microsoft.AspNetCore.Mvc;

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
            var user = _context.NguoiDung
                .FirstOrDefault(u => u.email == request.Email && u.matKhauHash == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu" });
            }

            var token = _authService.GenerateToken(user);
            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
