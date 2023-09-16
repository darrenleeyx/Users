using FluentValidation;
using FluentValidation.Results;

namespace Users.Application.Validators;

public abstract class ThrowIfInvalidValidator<T> : AbstractValidator<T>
{
    // This is used to make sure that all invalid validation
    // will raise an exception, especially for automatic validation approach
    // where it uses the ASP.NET validation pipeline
    //
    // This will allow all validation exceptions to be captured
    // in the exception handling middleware
    public override ValidationResult Validate(ValidationContext<T> context)
    {
        var result = base.Validate(context);

        if (result.IsValid == false)
        {
            RaiseValidationException(context, result);
        }

        return result;
    }
}
