using TvShowTracker.DTOs; 

public class ActorWithTvShowsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<TvShowSummaryDto> TvShows { get; set; } = new();
}
