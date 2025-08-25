using bookingEvent.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bookingEvent.Services.Auth
{
    public class AuthService
    {
        public string GenerateToken(Account account)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(account),
                Expires = DateTime.UtcNow.AddMinutes(AuthSettings.AccessTokenExpiryMinutes),
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(Account account)
        {
            var claims = new ClaimsIdentity();

            // Username hoặc Email
            claims.AddClaim(new Claim(ClaimTypes.Name, account.Email ?? account.Name));

            // ID
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()));

            // Role từ AccountGroup
            if (account.AccountGroup != null && !string.IsNullOrEmpty(account.AccountGroup.Name))
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, account.AccountGroup.Name));
            }

            claims.AddClaim(new Claim("AccountGroupId", account.AccountGroupId.ToString()));


            return claims;
        }
    }
}
