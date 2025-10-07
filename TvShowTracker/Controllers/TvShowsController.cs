using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using TvShowTracker.Data;
using TvShowTracker.DTOs;
using TvShowTracker.Models;
using TvShowTracker.Services;
using Microsoft.AspNetCore.Authorization;

namespace TvShowTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TvShowsController : ControllerBase
    {
        private readonly TvShowService _tvShowService;
        private readonly ApplicationDbContext _context;

        public TvShowsController(TvShowService tvShowService, ApplicationDbContext context)
        {
            _tvShowService = tvShowService;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shows = await _context.TvShows
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
                })
                .ToListAsync();

            return Ok(shows);
        }

        [HttpGet("with-episodes")]
        public IActionResult GetAllWithEpisodes()
        {
            var result = _tvShowService.GetAllWithEpisodes();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTvShow([FromBody] TvShowCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Título é obrigatório.");

            var created = await _tvShowService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("by-genre/{genreName}")]
        public IActionResult GetByGenre(string genreName)
        {
            var result = _tvShowService.GetByGenre(genreName);
            return Ok(result);
        }

        [HttpGet("sorted")]
        public IActionResult GetSorted([FromQuery] string orderBy = "title", [FromQuery] string direction = "asc")
        {
            var result = _tvShowService.GetSorted(orderBy, direction);
            return Ok(result);
        }

        [HttpGet("{tvShowId}/actors")]
        public IActionResult GetActorsByTvShow(int tvShowId)
        {
            var result = _tvShowService.GetActorsByTvShowId(tvShowId);
            return Ok(result);
        }

        [HttpGet("by-actor/{actorId}")]
        public IActionResult GetTvShowsByActor(int actorId)
        {
            var result = _tvShowService.GetTvShowsByActorId(actorId);
            return Ok(result);
        }

        [HttpGet("actors")]
        public IActionResult GetAllActors()
        {
            var result = _context.Actors
                .Select(a => new { a.Id, a.Name })
                .ToList();

            return Ok(result);
        }

        [HttpPost("favorites")]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteDto dto)
        {
            if (dto.UserId <= 0 || dto.TvShowId <= 0)
                return BadRequest("IDs inválidos.");

            var success = await _tvShowService.AddFavoriteAsync(dto.UserId, dto.TvShowId);
            return success ? Ok("Favorito adicionado.") : Conflict("Já está nos favoritos.");
        }

        [HttpDelete("favorites")]
        public async Task<IActionResult> RemoveFavorite([FromBody] FavoriteDto dto)
        {
            if (dto.UserId <= 0 || dto.TvShowId <= 0)
                return BadRequest("IDs inválidos.");

            var success = await _tvShowService.RemoveFavoriteAsync(dto.UserId, dto.TvShowId);
            return success ? Ok("Favorito removido.") : NotFound("Favorito não encontrado.");
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Parâmetros de paginação inválidos.");

            var query = _context.TvShows
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var items = await query
                .Select(t => new TvShowSummaryDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    ReleaseDate = t.ReleaseDate,
                    ImageUrl = t.ImageUrl,
                    VideoUrl = t.VideoUrl,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            var totalItems = await _context.TvShows.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return Ok(new
            {
                items,
                totalPages
            });
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, "Returns detailed information about a TV show", typeof(TvShowDetailedDto))]
        [SwaggerResponseExample(200, typeof(TvShowDetailedDtoExample))]
        public ActionResult<TvShowDetailedDto> GetById(int id)
        {
            var result = _tvShowService.GetDetailedById(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost("genres")]
        public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Nome do género é obrigatório.");

            if (_context.Genres.Any(g => g.Name.ToLower() == dto.Name.ToLower()))
                return Conflict("Género já existe.");

            var genre = new Genre { Name = dto.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByGenre), new { genreName = genre.Name }, genre);
        }

        [HttpGet("images")]
        public IActionResult GetAllMediaUrls()
        {
            var result = _context.TvShows
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.ImageUrl,
                    t.VideoUrl
                }).ToList();

            return Ok(result);
        }

        [HttpPost("{tvShowId}/episodes")]
        public async Task<IActionResult> AddEpisode(int tvShowId, [FromBody] EpisodeCreateDto dto)
        {
            if (tvShowId != dto.TvShowId)
                return BadRequest("ID da série não corresponde.");

            var exists = await _context.TvShows.AnyAsync(t => t.Id == tvShowId);
            if (!exists)
                return NotFound("Série não encontrada.");

            var episode = new Episode
            {
                Title = dto.Title,
                SeasonNumber = dto.SeasonNumber,
                EpisodeNumber = dto.EpisodeNumber,
                AirDate = dto.AirDate,
                ImageUrl = dto.ImageUrl,
                VideoUrl = dto.VideoUrl,
                TvShowId = tvShowId
            };

            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEpisodeById), new { id = episode.Id }, episode);
        }

        [HttpGet("episodes/{id}")]
        public async Task<IActionResult> GetEpisodeById(int id)
        {
            var episode = await _context.Episodes
                .Include(e => e.TvShow)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null)
                return NotFound();

            return Ok(new
            {
                episode.Id,
                episode.Title,
                episode.SeasonNumber,
                episode.EpisodeNumber,
                episode.AirDate,
                episode.ImageUrl,
                episode.VideoUrl,
                TvShow = new
                {
                    TvShow = episode.TvShow != null
    ? new
    {
        Id = episode.TvShow.Id,
        Title = episode.TvShow.Title ?? "Título indisponível"
    }
    : null

                }
            });
        }

        [HttpGet("episodes/paged")]
        public async Task<IActionResult> GetPagedEpisodes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Episodes
                .OrderByDescending(e => e.AirDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var items = await query
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.SeasonNumber,
                    e.EpisodeNumber,
                    e.AirDate,
                    e.ImageUrl,
                    e.VideoUrl,
                    e.TvShowId
                })
                .ToListAsync();

            var totalItems = await _context.Episodes.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return Ok(new { items, totalPages });
        }

        [HttpGet("by-title/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var tvShow = await _context.TvShows
                .Include(t => t.Genres)
                .Include(t => t.Actors)
                .Include(t => t.Episodes)
                .FirstOrDefaultAsync(t => t.Title == title);

            if (tvShow == null)
                return NotFound("Série não encontrada.");

            return Ok(tvShow);
        }

    }
}
