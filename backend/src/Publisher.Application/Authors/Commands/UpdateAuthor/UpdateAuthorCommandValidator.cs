using FluentValidation;

namespace Publisher.Application.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(c => c.RoyaltyRate)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
