using TvShowTracker.DTOs; 
public class GenreWithTvShowsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<TvShowSummaryDto> TvShows { get; set; } = new();
}
