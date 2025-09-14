using BCrypt.Net;
using bookingEvent.Const;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
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
        private readonly IAuthRepository _authService;
        private readonly AppSettingService _settingService;

        public AuthController(IAuthRepository authService, AppSettingService settingService)
        {
            _authService = authService;
            _settingService = settingService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var validateResult = await _authService.Validate(dto.Password);
            if (!validateResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Mật khẩu không hợp lệ",
                    Errors = validateResult.Errors
                });
            }

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

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            try
            {
                var message = await _authService.ConfirmEmailAsync(token);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message ?? "Có lỗi xảy ra khi xác thực email" });
            }
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
