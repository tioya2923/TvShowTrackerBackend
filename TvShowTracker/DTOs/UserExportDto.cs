public class UserExportDto
{
    public int UserId { get; set; }
    public required string Email { get; set; }
    public List<string> FavoriteTitles { get; set; } = new();
}
