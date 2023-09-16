namespace Users.Api.Contracts.Responses;

/// <summary>
/// The returned response when querying for a list of users
/// </summary>
public class UsersResponse
{
    /// <summary>
    /// The list containing all of the queried users
    /// </summary>
    public required IEnumerable<UserResponse> Items { get; init; } = Enumerable.Empty<UserResponse>();
}
