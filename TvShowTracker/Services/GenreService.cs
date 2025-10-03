using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TvShowTracker.Data;
using TvShowTracker.DTOs;
using TvShowTracker.Interfaces;
using TvShowTracker.Models;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;

    public GenreService(ApplicationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public List<GenreDto> GetGenres()
    {
        const string cacheKey = "genres_list";

        if (_cache.TryGetValue(cacheKey, out List<GenreDto>? genres) && genres != null)
            return genres;

        genres = _context.Genres
            .OrderBy(g => g.Name)
            .Select(g => new GenreDto
            {
                Id = g.Id,
                Name = g.Name
            })
            .ToList();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(6));

        _cache.Set(cacheKey, genres, cacheEntryOptions);

        return genres;
    }

    public async Task<Genre> CreateGenreAsync(GenreCreateDto dto)

    {

        if (_context.Genres.Any(g => g.Name.ToLower() == dto.Name.ToLower()))
            throw new InvalidOperationException("Género já existe.");
            
        var genre = new Genre { Name = dto.Name };
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public Genre? GetGenreById(int id)
    {
        return _context.Genres.Find(id);
    }

}
