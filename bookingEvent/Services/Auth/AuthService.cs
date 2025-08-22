using bookingEvent.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bookingEvent.Services.Auth
{
    public class AuthService
    {
        public string GenerateToken(NguoiDung user)
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

        private static ClaimsIdentity GenerateClaims(NguoiDung user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.email));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ma.ToString()));

            // Role-based access control
            if (!string.IsNullOrEmpty(user.vaiTro))
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, user.vaiTro));
            }

            return claims;
        }
    }
}
