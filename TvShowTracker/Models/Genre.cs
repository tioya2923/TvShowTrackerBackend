public class Genre
{
    public int Id { get; set; } // ← gerado automaticamente
    public required string Name { get; set; }

    public ICollection<TvShow> TvShows { get; set; } = new List<TvShow>();
}
