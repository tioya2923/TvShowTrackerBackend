using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Data;
using TvShowTracker.DTOs;
using TvShowTracker.Models;

namespace TvShowTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var actors = _context.Actors
                .Select(a => new ActorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    BirthDate = a.BirthDate,
                    PhotoUrl = a.PhotoUrl,
                    Biography = a.Biography
                }).ToList();

            return Ok(actors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActorCreateDto dto)
        {
            var actor = new Actor
            {
                Name = dto.Name,
                BirthDate = DateTime.SpecifyKind(dto.BirthDate, DateTimeKind.Utc),
                PhotoUrl = dto.PhotoUrl,
                Biography = dto.Biography
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = actor.Id }, new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthDate = actor.BirthDate,
                PhotoUrl = actor.PhotoUrl,
                Biography = actor.Biography
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetById(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.TvShows)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor is null) return NotFound();

            return new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthDate = actor.BirthDate,
                PhotoUrl = actor.PhotoUrl,
                Biography = actor.Biography,
                TvShows = actor.TvShows.Select(tv => new TvShowDto
                {
                    Id = tv.Id,
                    Title = tv.Title,
                    ImageUrl = tv.ImageUrl
                }).ToList()
            };
        }
    }
}
