using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Data;
using TvShowTracker.DTOs;
using TvShowTracker.Models;

public class TvShowService
{
    private readonly ApplicationDbContext _context;

    public TvShowService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<TvShowDetailedDto> GetAllWithEpisodes()
    {
        return _context.TvShows
            .Include(t => t.Episodes)
            .Include(t => t.Genres)
            .Include(t => t.Actors)
            .Select(t => new TvShowDetailedDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ReleaseDate = t.ReleaseDate,
                ImageUrl = t.ImageUrl,
                VideoUrl = t.VideoUrl,
                Genres = t.Genres.Select(g => new GenreDto { Id = g.Id, Name = g.Name }).ToList(),
                Actors = t.Actors.Select(a => new ActorDto { Id = a.Id, Name = a.Name }).ToList(),
                Episodes = t.Episodes.Select(e => new EpisodeDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    SeasonNumber = e.SeasonNumber,
                    EpisodeNumber = e.EpisodeNumber,
                    AirDate = e.AirDate,
                    ImageUrl = e.ImageUrl,
                    VideoUrl = e.VideoUrl,
                    TvShowId = e.TvShowId
                }).ToList()
            })
            .ToList();
    }

    public async Task<TvShowDto> CreateAsync(TvShowCreateDto dto)
    {
        var genres = await _context.Genres.Where(g => dto.GenreIds.Contains(g.Id)).ToListAsync();
        var actors = await _context.Actors.Where(a => dto.ActorIds.Contains(a.Id)).ToListAsync();

        var tvShow = new TvShow
        {
            Title = dto.Title,
            Description = dto.Description ?? "",
            ReleaseDate = DateTime.SpecifyKind(dto.ReleaseDate, DateTimeKind.Utc),
            ImageUrl = dto.ImageUrl,
            VideoUrl = dto.VideoUrl,
            CreatedAt = DateTime.UtcNow,
            Genres = genres,
            Actors = actors
        };

        _context.TvShows.Add(tvShow);
        await _context.SaveChangesAsync();

        return new TvShowDto
        {
            Id = tvShow.Id,
            Title = tvShow.Title,
            Description = tvShow.Description,
            ReleaseDate = tvShow.ReleaseDate,
            ImageUrl = tvShow.ImageUrl,
            VideoUrl = tvShow.VideoUrl,
            CreatedAt = tvShow.CreatedAt
        };
    }

    public TvShowDetailedDto? GetDetailedById(int id)
    {
        var tvShow = _context.TvShows
            .Include(t => t.Episodes)
            .Include(t => t.Genres)
            .Include(t => t.Actors)
            .Include(t => t.Favorites)
            .FirstOrDefault(t => t.Id == id);

        if (tvShow is null) return null;

        return new TvShowDetailedDto
        {
            Id = tvShow.Id,
            Title = tvShow.Title,
            Description = tvShow.Description,
            ReleaseDate = tvShow.ReleaseDate,
            ImageUrl = tvShow.ImageUrl,
            VideoUrl = tvShow.VideoUrl,
            CreatedAt = tvShow.CreatedAt,

            Genres = tvShow.Genres.Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToList(),

            Actors = tvShow.Actors.Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                PhotoUrl = a.PhotoUrl,
                Biography = a.Biography
            }).ToList(),

            Episodes = tvShow.Episodes.Select(e => new EpisodeDto
            {
                Id = e.Id,
                Title = e.Title,
                SeasonNumber = e.SeasonNumber,
                EpisodeNumber = e.EpisodeNumber,
                AirDate = e.AirDate,
                ImageUrl = e.ImageUrl,   // ✅ Correção aplicada
                VideoUrl = e.VideoUrl,   // ✅ Correção aplicada
                TvShowId = e.TvShowId
            }).ToList(),

            Favorites = tvShow.Favorites.Select(f => new FavoriteDto
            {
                Id = f.Id,
                UserId = f.UserId,
                TvShowId = f.TvShowId
            }).ToList()
        };
    }

    public List<TvShowSummaryDto> GetByGenre(string genreName)
    {
        return _context.TvShows
            .Include(t => t.Genres)
            .Where(t => t.Genres.Any(g => g.Name.ToLower() == genreName.ToLower()))
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TvShowSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ReleaseDate = t.ReleaseDate,
                ImageUrl = t.ImageUrl,
                VideoUrl = t.VideoUrl,
                CreatedAt = t.CreatedAt
            }).ToList();
    }

    public List<TvShowSummaryDto> GetSorted(string orderBy, string direction)
    {
        var query = _context.TvShows.AsQueryable();

        query = (orderBy.ToLower(), direction.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(t => t.Title),
            ("title", "desc") => query.OrderByDescending(t => t.Title),
            ("releasedate", "asc") => query.OrderBy(t => t.ReleaseDate),
            ("releasedate", "desc") => query.OrderByDescending(t => t.ReleaseDate),
            ("createdat", "asc") => query.OrderBy(t => t.CreatedAt),
            ("createdat", "desc") => query.OrderByDescending(t => t.CreatedAt),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        return query.Select(t => new TvShowSummaryDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            ReleaseDate = t.ReleaseDate,
            ImageUrl = t.ImageUrl,
            VideoUrl = t.VideoUrl,
            CreatedAt = t.CreatedAt
        }).ToList();
    }

    public List<ActorDto> GetActorsByTvShowId(int tvShowId)
    {
        return _context.TvShows
            .Include(t => t.Actors)
            .Where(t => t.Id == tvShowId)
            .SelectMany(t => t.Actors)
            .Select(a => new ActorDto
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                PhotoUrl = a.PhotoUrl,
                Biography = a.Biography
            }).ToList();
    }

    public List<TvShowSummaryDto> GetTvShowsByActorId(int actorId)
    {
        return _context.Actors
            .Include(a => a.TvShows)
            .Where(a => a.Id == actorId)
            .SelectMany(a => a.TvShows)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TvShowSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ReleaseDate = t.ReleaseDate,
                ImageUrl = t.ImageUrl,
                VideoUrl = t.VideoUrl,
                CreatedAt = t.CreatedAt
            }).ToList();
    }

    public async Task<bool> AddFavoriteAsync(int userId, int tvShowId)
    {
        var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.TvShowId == tvShowId);

        if (exists) return false;

        var favorite = new Favorite { UserId = userId, TvShowId = tvShowId };
        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFavoriteAsync(int userId, int tvShowId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.TvShowId == tvShowId);

        if (favorite == null) return false;

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
        return true;
    }

    public List<TvShowSummaryDto> GetPaged(int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;

        return _context.TvShows
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .Select(t => new TvShowSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                ReleaseDate = t.ReleaseDate,
                ImageUrl = t.ImageUrl,
                VideoUrl = t.VideoUrl,
                CreatedAt = t.CreatedAt
            }).ToList();
    }
}
