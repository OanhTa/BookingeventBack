using BCrypt.Net;
using bookingEvent.Const;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Services;
using bookingEvent.Services.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly AppSettingService _settingService;

        public AuthController(AuthService authService, AppSettingService settingService)
        {
            _authService = authService;
            _settingService = settingService;
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

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto dto)
        {
            bool preventEmailEnumeration = await _settingService.IsTrueAsync(AppSettingNames.PreventEmailEnumeration);

            await _authService.RequestPasswordResetAsync(dto.Email);

            if (preventEmailEnumeration)
            {
                // Luôn trả message chung
                return Ok(new { message = "Nếu email tồn tại, kiểm tra mail để  đặt lại mật khẩu" });
            }
            else
            {
                return Ok(new { message = "Kiểm tra mail để  đặt lại mật khẩu." });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            if (!success)
                return BadRequest(new { message = "Invalid or expired token." });

            return Ok(new { message = "Password has been successfully reset." });
        }

    }
}

public class RequestPasswordResetDto
{
    public string Email { get; set; } = null!;
}


public class ResetPasswordDto
{
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}
