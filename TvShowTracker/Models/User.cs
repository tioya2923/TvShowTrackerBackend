using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TvShowTracker.Models
{

    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        public string? Name { get; set; }
        public string? Username { get; set; } 

        public bool ConsentGiven { get; set; }
        public DateTime? ConsentDate { get; set; }

        public List<Favorite> Favorites { get; set; } = new();
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }

    }


}
