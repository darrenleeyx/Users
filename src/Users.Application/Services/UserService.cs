using FluentValidation;
using Users.Application.Models;
using Users.Application.Repositories;

namespace Users.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _userValidator;

    public UserService(IUserRepository userRepository, IValidator<User> userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    public Task<bool> ContainsUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _userRepository.ContainsUsernameAsync(username, cancellationToken);
    }

    public async Task<bool> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _userValidator.ValidateAndThrowAsync(user, cancellationToken);
        return await _userRepository.CreateAsync(user, cancellationToken);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _userRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _userRepository.GetAllAsync(cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByIdAsync(id, cancellationToken);
    }
}
