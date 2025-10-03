using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Interfaces;
using TvShowTracker.DTOs;
using TvShowTracker.Models;

namespace TvShowTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/genre
        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _genreService.GetGenres();
            return Ok(genres);
        }

        // POST: api/genre
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto dto)
        {
            try
            {
                var created = await _genreService.CreateGenreAsync(dto);
                return CreatedAtAction(nameof(GetGenreById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        // GET: api/genre/{id}
        [HttpGet("{id}")]
        public IActionResult GetGenreById(int id)
        {
            var genre = _genreService.GetGenreById(id);
            return genre is null ? NotFound() : Ok(genre);
        }
    }
}
