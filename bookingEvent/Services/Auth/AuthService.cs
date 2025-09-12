using bookingEvent.Const;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bookingEvent.Services.Auth
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettingService _settingService;
        private readonly EmailService _emailService;

        public AuthService(ApplicationDbContext context, AppSettingService settingService, EmailService emailService)
        {
            _context = context;
            _settingService = settingService;
            _emailService = emailService;
        }

        public string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddMinutes(AuthSettings.AccessTokenExpiryMinutes),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            // quyền riêng của user
            foreach (var up in user.UserPermissions)
            {
                claims.AddClaim(new Claim("permission", up.Permission.Name));
            }

            // role + quyền từ role
            foreach (var ur in user.UserRoles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, ur.Role.Name));

                foreach (var rp in ur.Role.RolePermissions)
                {
                    claims.AddClaim(new Claim("permission", rp.Permission.Name));
                }
            }

            return claims;
        }

        public async Task<User?> Register(string username, string email, string password)
        {
            var isSelfRegistrationEnabled = await _settingService.IsTrueAsync(AppSettingNames.IsSelfRegistrationEnabled);
            if (isSelfRegistrationEnabled)
            {
                throw new InvalidOperationException("Tự đăng ký tài khoản đã bị vô hiệu hóa.");
            }
            if (_context.Users.Any(u => u.Email == email)) return null;
            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            var token = Guid.NewGuid().ToString();
            user.EmailConfirmedToken = token;


            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            var resetLink = $"http://localhost:4200/email-verify?token={token}";
            await _emailService.SendEmailAsync(email, "Xác thực email", $"Click link để xác thực email: {resetLink}");
            return user;
        }

        public async Task<LoginResponseDto?> Login(string email, string password)
        {
            var enableLocalLogin = await _settingService.IsTrueAsync(AppSettingNames.EnableLocalLogin);
            if (enableLocalLogin)
            {
                throw new InvalidOperationException("Đăng nhập bằng tài khoản nội bộ đã bị vô hiệu hóa.");
            }

            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                await HandleFailedLoginAsync(user);
                return null;
            }

            var token = GenerateToken(user);

            // cập nhật hạn token 7 ngày
            user.AccessToken = token;
            user.TokenExpireTime = DateTime.UtcNow.AddDays(7);
            user.AccessFailedCount = 0;
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                Phone = user.Phone,
                AvatarUrl = user.AvatarUrl,
                Expiry = user.TokenExpireTime.Value,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return;

            var token = Guid.NewGuid().ToString();

            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);

            _context.Entry(user).Property(u => u.ResetPasswordToken).IsModified = true;
            _context.Entry(user).Property(u => u.ResetPasswordTokenExpiry).IsModified = true;
            await _context.SaveChangesAsync();

            var resetLink = $"http://localhost:4200/reset-password?token={token}";
            await _emailService.SendEmailAsync(email, "Đặt lại mật khẩu", $"Click link để đổi mật khẩu: {resetLink}");
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token &&
                                                                      u.ResetPasswordTokenExpiry > DateTime.UtcNow);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Xoá token sau khi sử dụng
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            _context.Entry(user).Property(u => u.PasswordHash).IsModified = true;
            _context.Entry(user).Property(u => u.ResetPasswordToken).IsModified = true;
            _context.Entry(user).Property(u => u.ResetPasswordTokenExpiry).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<string> ConfirmEmailAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token không hợp lệ");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailConfirmedToken == token);

            if (user == null)
                throw new Exception("Người dùng không tồn tại hoặc token sai");

            user.EmailConfirmed = true;
            user.EmailConfirmedAt = DateTime.UtcNow;
            user.EmailConfirmedToken = null;
            await _context.SaveChangesAsync();
            return "Xác thực email thành công!";
        }

        public async Task<PasswordValidatorResult> Validate(string password)
        {
            var minLengthStr = await _settingService.GetValueAsync(AppSettingNames.PasswordMinLength);
            int minLength = int.TryParse(minLengthStr, out var ml) ? ml : 6;

            var minUniqueStr = await _settingService.GetValueAsync(AppSettingNames.PasswordUniqueChars);
            int minUniqueChars = int.TryParse(minUniqueStr, out var mu) ? mu : 3;

            var requireSpecialChar = await _settingService.IsTrueAsync(AppSettingNames.PasswordRequireNonAlphanumeric);

            var requireLowercaseStr = await _settingService.GetValueAsync(AppSettingNames.PasswordRequireLowercase);
            bool requireLowercase = bool.TryParse(requireLowercaseStr, out var rl) && rl;

            var requireUppercaseStr = await _settingService.GetValueAsync(AppSettingNames.PasswordRequireUppercase);
            bool requireUppercase = bool.TryParse(requireUppercaseStr, out var ru) && ru;

            var requireDigitStr = await _settingService.GetValueAsync(AppSettingNames.PasswordRequireDigit);
            bool requireDigit = bool.TryParse(requireDigitStr, out var rd) && rd;

            var result = new PasswordValidatorResult();

            if (string.IsNullOrWhiteSpace(password))
            {
                result.Errors.Add("Mật khẩu không được để trống.");
                result.IsValid = false;
                return result;
            }

            if (password.Length < minLength)
                result.Errors.Add($"Mật khẩu phải có ít nhất {minLength} ký tự.");

            if (password.Distinct().Count() < minUniqueChars)
                result.Errors.Add($"Mật khẩu phải có ít nhất {minUniqueChars} ký tự khác nhau.");

            if (requireSpecialChar && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                result.Errors.Add("Mật khẩu phải chứa ít nhất một ký tự đặc biệt.");

            if (requireLowercase && !password.Any(char.IsLower))
                result.Errors.Add("Mật khẩu phải chứa ít nhất một chữ thường (a-z).");

            if (requireUppercase && !password.Any(char.IsUpper))
                result.Errors.Add("Mật khẩu phải chứa ít nhất một chữ hoa (A-Z).");

            if (requireDigit && !password.Any(char.IsDigit))
                result.Errors.Add("Mật khẩu phải chứa ít nhất một chữ số (0-9).");

            result.IsValid = !result.Errors.Any();
            return result;
        }

        private async Task HandleFailedLoginAsync(User user)
        {
            var maxFailed = await _settingService.GetValueAsync(AppSettingNames.MaxFailedAccess);
            var lockDuration = await _settingService.GetValueAsync(AppSettingNames.LockoutDuration);

            // ép kiểu sang int
            int maxFailedCount = Convert.ToInt32(maxFailed);
            int lockDurationMinutes = Convert.ToInt32(lockDuration);

            user.AccessFailedCount += 1;

            if (user.AccessFailedCount >= maxFailedCount)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(lockDurationMinutes);
                user.AccessFailedCount = 0;
            }

            await _context.SaveChangesAsync();
        }

    }

    public class PasswordValidatorResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}