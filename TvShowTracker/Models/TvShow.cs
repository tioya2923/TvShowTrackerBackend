public class TvShow
{
    public int Id { get; set; } // Gerado automaticamente

    public required string Title { get; set; }

    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; } // URL da imagem

    public string? VideoUrl { get; set; } // URL do vídeo breve

    public DateTime ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Usado para ordenação

    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
