namespace Users.Application.Models;

public class User
{
    public required Guid Id { get; init; }
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required DateTime DateJoined { get; set; }
}
