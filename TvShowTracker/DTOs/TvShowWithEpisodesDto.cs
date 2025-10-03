namespace TvShowTracker.DTOs
{
    public class TvShowWithEpisodesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EpisodeDto> Episodes { get; set; } = new();
    }
}
