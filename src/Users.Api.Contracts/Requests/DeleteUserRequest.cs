namespace Users.Api.Contracts.Requests;

/// <summary>
/// A request to delete a user
/// </summary>
public class DeleteUserRequest
{
    /// <summary>
    /// The id of the user
    /// </summary>
    public required Guid Id { get; init; }
}
