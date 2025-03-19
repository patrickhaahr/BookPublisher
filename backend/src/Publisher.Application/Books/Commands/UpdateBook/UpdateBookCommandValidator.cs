using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
         RuleFor(c => c.Id)
             .NotEmpty()
             .WithMessage("Book ID is required");
        // model binding handles GUID format first, so we don't need to validate it here
        //     .Must(Validation.IsValidGuid)
        //     .WithMessage("Book ID must be a valid GUID");

        RuleFor(c => c.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(c => c.PublishDate)
            .NotEmpty()
            .WithMessage("Publish date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publish date cannot be in the future");

        RuleFor(c => c.BasePrice)
            .GreaterThan(0)
            .WithMessage("Base price must be greater than 0");
    }
}