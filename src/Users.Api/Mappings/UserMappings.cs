using Users.Api.Contracts.Requests;
using Users.Api.Contracts.Responses;
using Users.Application.Models;
using Users.Application.Providers;

namespace Users.Api.Mappings;

public static class UserMappings
{
    public static User MapToUser(this CreateUserRequest request, IDateTimeProvider dateTimeProvider)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateJoined = dateTimeProvider.Now
        };
    }

    public static UserResponse MapToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            DateJoined = user.DateJoined
        };
    }

    public static UsersResponse MapToResponse(this IEnumerable<User> users)
    {
        return new UsersResponse
        {
            Items = users.Select(u => u.MapToResponse())
        };
    }
}
