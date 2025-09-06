using bookingEvent.Data;
using bookingEvent.Model;
using bookingEvent.Services.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using bookingEvent.DTO;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _authService.Register(dto.UserName, dto.Email, dto.Password);
            if (user == null) return BadRequest("Email đã tồn tại");
            return Ok(new { message = "Đăng ký thành công" });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var response = await _authService.Login(dto.Email, dto.Password);
            if (response == null)
                return Unauthorized("Sai tài khoản hoặc mật khẩu");

            return Ok(response);
        }

    }
}
