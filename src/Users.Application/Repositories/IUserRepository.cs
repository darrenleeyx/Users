using Users.Application.Models;

namespace Users.Application.Repositories;

public interface IUserRepository
{
    Task<bool> ContainsUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
