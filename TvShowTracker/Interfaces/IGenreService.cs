using TvShowTracker.DTOs;
using TvShowTracker.Models;

namespace TvShowTracker.Interfaces
{
    public interface IGenreService
    {
        List<GenreDto> GetGenres();
        Task<Genre> CreateGenreAsync(GenreCreateDto dto);
        Genre? GetGenreById(int id);
    }
}
