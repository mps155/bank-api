using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IMongoCollection<Owner> _users;
        private readonly IMongoCollection<RefreshToken> _refreshTokens;

        public AuthController(JwtService jwtService,
                              IMongoClient mongoClient,
                              IOptions<MongoDBSettings> mongoDBSettings)
        {
            _jwtService = jwtService;
            var database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _users = database.GetCollection<Owner>("Owner");
            _refreshTokens = database.GetCollection<RefreshToken>("RefreshTokens");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterOwnerRequest request)
        {
            // Verifica se o nome de usuário já existe
            var existingUser = await _users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Nome de usuário já está em uso.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new Owner
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                Active = true,
                Wallets = []
            };

            await _users.InsertOneAsync(newUser);

            return Ok("Usuário criado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null || !VerifyPassword(request.Password, user.Password))
                return Unauthorized("Usuário ou senha incorretos");

            var jwtToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(user._id);

            await _refreshTokens.InsertOneAsync(refreshToken);

            return Ok(new
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var refreshToken = await _refreshTokens.Find(rt => rt.Token == request.RefreshToken && !rt.IsRevoked).FirstOrDefaultAsync();

            if (refreshToken == null || refreshToken.Expiration < DateTime.UtcNow)
                return Unauthorized("Token de atualização inválido ou expirado");

            var user = await _users.Find(u => u._id == refreshToken.UserId).FirstOrDefaultAsync();
            if (user == null)
                return Unauthorized("Usuário não encontrado");

            var newJwtToken = _jwtService.GenerateJwtToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user._id);

            refreshToken.IsRevoked = true;
            await _refreshTokens.ReplaceOneAsync(rt => rt.Token == request.RefreshToken, refreshToken);
            await _refreshTokens.InsertOneAsync(newRefreshToken);

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken.Token
            });
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
    }
}
