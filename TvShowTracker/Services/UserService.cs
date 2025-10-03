using System;
using System.Linq;
using TvShowTracker.Data;
using TvShowTracker.Interfaces;
using TvShowTracker.Models;

namespace TvShowTracker.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool DeleteUserData(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            var favorites = _context.Favorites.Where(f => f.UserId == userId).ToList();
            if (favorites.Any())
                _context.Favorites.RemoveRange(favorites);

            // Outras entidades relacionadas podem ser removidas aqui

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public void RegisterConsent(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                throw new InvalidOperationException("Utilizador não encontrado.");

            user.ConsentGiven = true;
            user.ConsentDate = DateTime.UtcNow;

            _context.SaveChanges();
        }
    }
}
