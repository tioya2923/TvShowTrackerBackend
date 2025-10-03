public class EpisodeCreateDto
{
    public required string Title { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public DateTime AirDate { get; set; }
    public string? ImageUrl { get; set; }  
        public string? VideoUrl { get; set; }  
    public int TvShowId { get; set; }
}
