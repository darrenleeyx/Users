using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Net;
using Users.Api.Contracts.Requests;
using Users.Api.Contracts.Responses;
using Users.Api.Controllers;
using Users.Api.Mappings;
using Users.Application.Models;
using Users.Application.Providers;
using Users.Application.Services;
using Xunit;

namespace Users.Api.Tests.Unit;

public class UsersControllerTests
{
    private UsersController _sut;
    private IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private IUserService _userService = Substitute.For<IUserService>();

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
    public UsersControllerTests()
    {
        _sut = new UsersController(_userService, _dateTimeProvider);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkAndObject_WhenUserExists()
    {
        // Arrange
        var user = _userGenerator.Generate();
        _userService.GetByIdAsync(user.Id).Returns(user);
        var expectedResponse = user.MapToResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetById(user.Id);

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = (NotFoundResult)await _sut.GetById(Guid.NewGuid());

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndObject_WhenUsersExist()
    {
        // Arrange
        var user1 = _userGenerator.Generate();
        var user2 = _userGenerator.Generate();
        var users = new List<User> { user1, user2 };
        _userService.GetAllAsync().Returns(users);
        var expectedResponse = users.MapToResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.As<UsersResponse>().Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkAndEmptyList_WhenNoUsersExist()
    {
        // Arrange
        var users = Enumerable.Empty<User>();
        _userService.GetAllAsync().Returns(users);
        var expectedResponse = users.MapToResponse();

        // Act
        var result = (OkObjectResult)await _sut.GetAll();

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.As<UsersResponse>().Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task Create_ShouldCreateUser_WhenCreateUserRequestIsValid()
    {
        // Arrange
        var request = _createUserRequestGenerator.Generate();
        _dateTimeProvider.Now.Returns(new DateTime(2020, 1, 1, 0, 0, 0));
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            DateJoined = _dateTimeProvider.Now
        };

        // updates the user object here inside of the endpoint
        // so that the id can be equivalent
        _userService.CreateAsync(Arg.Do<User>(x => user = x)).Returns(true);

        // Act
        var result = (CreatedAtActionResult)await _sut.Create(request);

        // Assert
        var expectedResponse = user.MapToResponse();
        result.StatusCode.Should().Be((int)HttpStatusCode.Created);
        result.Value.As<UserResponse>().Should().BeEquivalentTo(expectedResponse);
        result.RouteValues!["id"].Should().BeEquivalentTo(user.Id);
    }

    [Fact]
    public async Task Create_ShouldThrowValidationException_WhenCreateUserRequestIsInvalid()
    {
        // Arrange
        var invalidRequest = _createUserRequestGenerator.Generate();
        _userService.CreateAsync(Arg.Any<User>()).Returns(
            Task.FromException<bool>(new FluentValidation.ValidationException("")));

        // Act
        Func<Task> result = async () =>
        {
            var result = await _sut.Create(invalidRequest);
        };

        // Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(result);
    }

    [Fact]
    public async Task Delete_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var existingUser = _userGenerator.Generate();
        var request = new DeleteUserRequest { Id = existingUser.Id };
        _userService.DeleteByIdAsync(request.Id).Returns(true);

        // Act
        var result = (OkResult)await _sut.Delete(request);

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new DeleteUserRequest { Id = Guid.NewGuid() };
        _userService.DeleteByIdAsync(request.Id).Returns(false);

        // Act
        var result = (NotFoundResult)await _sut.Delete(request);

        // Assert
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}
