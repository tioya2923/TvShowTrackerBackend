public class Episode
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public string? Description { get; set; }

    public DateTime AirDate { get; set; }

     public string? ImageUrl { get; set; } 
    public string? VideoUrl { get; set; }  

    public int TvShowId { get; set; }
    public TvShow? TvShow { get; set; }

}
