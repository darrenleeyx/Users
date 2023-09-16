using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Users.Api.Contracts.Requests;
using Users.Application.Models;
using Users.Application.Providers;
using Users.Application.Repositories;
using Users.Application.Services;
using Users.Application.Validators;
using Xunit;

namespace Users.Application.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IValidator<User> _validator;

    private readonly Faker<User> _userGenerator = new Faker<User>()
        .RuleFor(x => x.Id, Guid.NewGuid)
        .RuleFor(x => x.Username, faker => faker.Person.UserName.ClampLength(8, 20))
        .RuleFor(x => x.Name, faker => faker.Person.FullName.ClampLength(8, 20))
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Phone, "12345678")
        .RuleFor(x => x.DateJoined, faker => faker.Person.DateOfBirth);

    private readonly Faker<CreateUserRequest> _createUserRequestGenerator = new Faker<CreateUserRequest>()
        .RuleFor(x => x.Username, faker => faker.Person.UserName.ClampLength(8, 20))
        .RuleFor(x => x.Name, faker => faker.Person.FullName.ClampLength(8, 20))
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.Phone, "12345678");

    public UserServiceTests()
    {
        _validator = new UserValidator(_userRepository, _dateTimeProvider);
        _sut = new UserService(_userRepository, _validator);
    }

    [Fact]
    public async Task ContainsUsernameAsync_ShouldReturnTrue_WhenUsernameExists()
    {
        // Arrange
        _userRepository.ContainsUsernameAsync("username").Returns(true);

        // Act
        var result = await _sut.ContainsUsernameAsync("username");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ContainsUsernameAsync_ShouldReturnFalse_WhenUsernameDoesNotExist()
    {
        // Arrange
        _userRepository.ContainsUsernameAsync("username").Returns(false);

        // Act
        var result = await _sut.ContainsUsernameAsync("username");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUsers_WhenUsersExists()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            _userGenerator.Generate(),
            _userGenerator.Generate()
        };
        _userRepository.GetAllAsync().Returns(expectedUsers);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WheNoUserExists()
    {
        // Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var expectedUser = _userGenerator.Generate();
        _userRepository.GetByIdAsync(expectedUser.Id).Returns(expectedUser);

        // Act
        var result = await _sut.GetByIdAsync(expectedUser.Id);

        // Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenDataIsValid()
    {
        // Arrange
        var request = _createUserRequestGenerator.Generate();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateJoined = new DateTime(2020, 1, 1, 0, 0, 0)
        };
        _dateTimeProvider.Now.Returns(new DateTime(2023, 1, 1, 0, 0, 0));
        _userRepository.CreateAsync(user).Returns(true);

        // Act
        var result = await _sut.CreateAsync(user);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowValidationException_WhenDataIsInvalid()
    {
        // Arrange
        var request = _createUserRequestGenerator.Generate();
        var invalidUser = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateJoined = new DateTime(3000, 1, 1, 0, 0, 0)
        };
        _dateTimeProvider.Now.Returns(new DateTime(2023, 1, 1, 0, 0, 0));
        _userRepository.CreateAsync(invalidUser).Returns(false);

        // Act
        Func<Task> result = async () =>
        {
            var result = await _sut.CreateAsync(invalidUser);
        };

        // Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var user = _userGenerator.Generate();
        _userRepository.DeleteByIdAsync(user.Id).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(user.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);

        // Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }
}
