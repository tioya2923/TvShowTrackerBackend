public class UserRegisterDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }

    public bool ConsentGiven { get; set; }
}
