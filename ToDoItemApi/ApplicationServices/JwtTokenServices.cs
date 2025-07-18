using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoItemApi.Models.Auth;

namespace ToDoItemApi.ApplicationServices
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(string userId, string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role) 
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiresInMinutes),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
