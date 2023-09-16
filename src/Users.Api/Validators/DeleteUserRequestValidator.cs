using FluentValidation;
using Users.Api.Contracts.Requests;
using Users.Application.Services;
using Users.Application.Validators;

namespace Users.Api.Validators;

public class DeleteUserRequestValidator : ThrowIfInvalidValidator<DeleteUserRequest>
{
    private readonly IUserService _userService;
    public DeleteUserRequestValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Id)
            .Must(BeAnExistingId).WithMessage("The id does not exist");
    }

    private bool BeAnExistingId(Guid id)
    {
        return _userService.ContainsIdAsync(id).Result;
    }
}
