using BankAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankAPI.Services
{
    public class JwtService
    {
        private readonly JwtConfig _config;

        public JwtService(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        public string GenerateJwtToken(Owner user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user._id),
                new Claim(ClaimTypes.Name, user.Email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(_config.ExpiresInMinutes),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string userId)
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userId,
                Expiration = DateTime.UtcNow.AddMinutes(5)
            };
        }
    }
}
