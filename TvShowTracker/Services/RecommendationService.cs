using TvShowTracker.Data;
using TvShowTracker.DTOs;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Services;
using Hangfire;



public class RecommendationService
{
    private readonly ApplicationDbContext _context;
    private readonly EmailService _emailService;

    public RecommendationService(ApplicationDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }
    public void EnqueueRecommendationEmail(int userId, string email)
{
    BackgroundJob.Enqueue(() => SendRecommendationsEmail(userId, email));
}


    public List<TvShowSummaryDto> GetRecommendationsForUser(int userId)
    {
        // 1. Obter géneros dos favoritos
        var favoriteGenres = _context.Favorites
            .Where(f => f.UserId == userId)
            .SelectMany(f => f.TvShow.Genres)
            .Distinct()
            .ToList();

        // 2. Obter séries com esses géneros que ainda não são favoritas
        var recommended = _context.TvShows
            .Include(t => t.Genres)
            .Where(t => t.Genres.Any(g => favoriteGenres.Contains(g)))
            .Where(t => !_context.Favorites.Any(f => f.UserId == userId && f.TvShowId == t.Id))
            .Select(t => new TvShowSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ReleaseDate = t.ReleaseDate
            })
            .Distinct()
            .ToList();

        return recommended;
    }

    public async Task SendRecommendationsEmail(int userId, string email)
    {
        var recommendations = GetRecommendationsForUser(userId);
        var body = string.Join("\n", recommendations.Select(r => $"- {r.Title} ({r.ReleaseDate:yyyy})"));

        await _emailService.SendAsync(email, "Novas séries recomendadas para ti", body);
    }
    
}
