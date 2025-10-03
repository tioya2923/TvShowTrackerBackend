using TvShowTracker.Models;

public class Favorite
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TvShowId { get; set; }
    public TvShow TvShow { get; set; } = null!;
}
