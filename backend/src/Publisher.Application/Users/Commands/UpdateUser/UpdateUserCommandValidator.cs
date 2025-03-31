using FluentValidation;

namespace Publisher.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.Username)
            .MaximumLength(50)
            .When(c => c.Username is not null);

        RuleFor(c => c.Email)
            .EmailAddress()
            .MaximumLength(100)
            .When(c => c.Email is not null);

        RuleFor(c => c.PasswordHash)
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .When(c => c.PasswordHash is not null)
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, and one number");
    }
}