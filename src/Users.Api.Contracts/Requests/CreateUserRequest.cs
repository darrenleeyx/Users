namespace Users.Api.Contracts.Requests;

/// <summary>
/// A request to create a user
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// The username of the user
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The name of the user
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The email of the user
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The phone number of the user
    /// </summary>
    public required string Phone { get; init; }
}
