using FluentValidation;
using System.Text.RegularExpressions;
using Users.Application.Models;
using Users.Application.Providers;
using Users.Application.Repositories;

namespace Users.Application.Validators;

public class UserValidator : ThrowIfInvalidValidator<User>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserValidator(IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(8, 20).WithMessage("The length of the username must be between 8 to 20 characters.")
            .MustAsync(BeAValidUsername).WithMessage("Username already exists");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(8, 20).WithMessage("The length of the username must be between 8 to 20 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .EmailAddress().WithMessage("A valid email address is required");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Length(8).WithMessage("Phone number can only contain 8 characters")
            .Must(BeAValidPhoneNumber).WithMessage("A valid phone number is required");

        RuleFor(x => x.DateJoined)
            .NotEmpty().WithMessage("Date joined is required")
            .Must(BeEarlierOrEqualToNow).WithMessage("A valid join date is required");
    }

    private async Task<bool> BeAValidUsername(string username, CancellationToken cancellationToken = default)
    {
        var usernameExists = await _userRepository.ContainsUsernameAsync(username);
        return usernameExists == false;
    }

    private bool BeAValidPhoneNumber(string phoneNumber)
    {
        // can only contain numerical character 0-9
        return new Regex("^[0-9]+$").IsMatch(phoneNumber);
    }

    private bool BeEarlierOrEqualToNow(DateTime dateTime)
    {
        return dateTime <= _dateTimeProvider.Now;
    }
}
