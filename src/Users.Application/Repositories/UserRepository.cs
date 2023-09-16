using Microsoft.Extensions.Configuration;
using Users.Application.Models;

namespace Users.Application.Repositories;

public class UserRepository : IUserRepository
{
    private List<User> _users = new();

    public UserRepository(List<User>? users)
    {
        if (users != null && users.Count > 0)
        {
            _users.AddRange(users);
        }
    }

    public Task<bool> ContainsIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.Any(u => u.Id == id));
    }

    public Task<bool> ContainsUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.Any(u => u.Username == username));
    }

    public Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = _users.SingleOrDefault(x => x.Id == id);

        if (user == null)
        {
            return Task.FromResult(false);
        }

        _users.Remove(user);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.AsEnumerable());
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = _users.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(user);
    }
}
