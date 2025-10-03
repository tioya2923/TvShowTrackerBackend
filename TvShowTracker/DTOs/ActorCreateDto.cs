public class ActorCreateDto
{
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Biography { get; set; }
}
