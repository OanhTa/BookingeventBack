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

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
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
            if (_context.Users.Any(u => u.Email == email)) return null;

            var hashed = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = hashed
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<LoginResponseDto?> Login(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

            var token = GenerateToken(user);

            // cập nhật hạn token 7 ngày
            user.AccessToken = token;
            user.TokenExpireTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.UserName,
                Expiry = user.TokenExpireTime.Value,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

    }
}
