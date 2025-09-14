using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Services.Auth;
using System.Security.Claims;

namespace bookingEvent.Repositories
{
    public interface IAuthRepository
    {
        Task<LoginResponseDto?> Login(string email, string password);
        Task<User?> Register(string username, string email, string password);
        Task RequestPasswordResetAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<string> ConfirmEmailAsync(string token);
        Task<PasswordValidatorResult> Validate(string password);
    }
}
