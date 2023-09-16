namespace Users.Api.Contracts.Responses;

/// <summary>
/// The returned response when querying for a user
/// </summary>
public class UserResponse
{
    /// <summary>
    /// The id of the user
    /// </summary>
    public required Guid Id { get; init; }

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

    /// <summary>
    /// The joined date of the user
    /// </summary>
    public required DateTime DateJoined { get; set; }
}
