public class TvShowCreateDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ImageUrl { get; set; }      
    public string? VideoUrl { get; set; }     
    public required List<int> GenreIds { get; set; }
    public required List<int> ActorIds { get; set; }
}
