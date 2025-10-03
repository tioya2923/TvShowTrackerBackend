using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Data;
using TvShowTracker.Models;
using TvShowTracker.Interfaces;
using TvShowTracker.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TvShowTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, IEmailService emailService, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists)
                return Conflict("Este email já está registado.");

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Name = dto.Name,
                ConsentGiven = dto.ConsentGiven,
                ConsentDate = dto.ConsentGiven ? DateTime.UtcNow : null
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { user.Id, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null || !VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Credenciais inválidas.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireHours = int.TryParse(_config["Jwt:ExpireHours"], out var hours) ? hours : 2;

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                token,
                profile = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    ConsentGiven = user.ConsentGiven
                }
            });
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return NotFound();

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                ConsentGiven = user.ConsentGiven
            };
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return NotFound("Email não encontrado.");

            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.TokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            await _emailService.SendAsync(user.Email, "Recuperação de palavra-passe",
                $"Clique aqui para redefinir: https://teusite.com/reset-password?token={token}");

            return Ok("Email enviado.");
        }

        // Métodos auxiliares (implementação real recomendada)
        private string HashPassword(string password)
        {
            // Implementar hashing seguro (ex: BCrypt)
            return password; // Placeholder
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Implementar verificação segura
            return password == hash; // Placeholder
        }
    }
}
