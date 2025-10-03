using TvShowTracker.DTOs;

public class TvShowDetailedDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GenreDto> Genres { get; set; } = new();
    public List<ActorDto> Actors { get; set; } = new();
    public List<EpisodeDto> Episodes { get; set; } = new();
    public List<FavoriteDto> Favorites { get; set; } = new();
}
