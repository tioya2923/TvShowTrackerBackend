public class Actor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Biography { get; set; }

    public List<TvShow> TvShows { get; set; } = new();
}
