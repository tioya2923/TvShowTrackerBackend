namespace TvShowTracker.Dtos;
public class UserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public bool ConsentGiven { get; set; }
    public DateTime? ConsentDate { get; set; }
}
