using Azure;
using bookingEvent.Const;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using bookingEvent.Services;
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
            try
            {
                var validateResult = await _authService.Validate(dto.Password);
                if (!validateResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = validateResult.Errors
                    });
                }

                var user = await _authService.Register(dto.UserName, dto.Email, dto.Password);
                if (user == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Email đã tồn tại"));
                }
                return Ok(ApiResponse<User>.SuccessResponse(user, "Bạn đã đăng ký thành công! Hãy kiểm tra hộp thư để xác nhận tài khoản"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Có lỗi xảy ra. Vui lòng thử lại."));
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var response = await _authService.Login(dto.Email, dto.Password);
                if (response == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Sai tài khoản hoặc mật khẩu"));
                }
                return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Đăng nhập thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Có lỗi xảy ra. Vui lòng thử lại."));
            }
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, [FromQuery] string userId)
        {
            var validateResult = await _authService.Validate(dto.PasswordNew);
            if (!validateResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = validateResult.Errors
                });
            }
            try
            {
                var response = await _authService.ChangePasswordAsync(userId, dto.PasswordCurrent, dto.PasswordNew);
                return Ok(ApiResponse<bool>.SuccessResponse(response, "Đổi mật khẩu thành công"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Có lỗi xảy ra. Vui lòng thử lại."));
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto dto)
        {
            bool preventEmailEnumeration = await _settingService.IsTrueAsync(AppSettingNames.PreventEmailEnumeration);

            await _authService.RequestPasswordResetAsync(dto.Email);

            if (preventEmailEnumeration)
            {
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
            var validateResult = await _authService.Validate(dto.NewPassword);
            if (!validateResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = validateResult.Errors
                });
            }
            var success = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);
            if (!success)
                return BadRequest(new { message = "Mã không hợp lệ hoặc hết hạn." });

            return Ok(new { message = "Đặt lại mật khẩu thành công" });
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

public class ChangePasswordDto
{
    public string PasswordCurrent { get; set; }
    public string PasswordNew { get; set; }
}
