using TvShowTracker.Data;
using TvShowTracker.DTOs;
using TvShowTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace TvShowTracker.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(UserRegistrationDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username);

            if (exists) return false;

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
