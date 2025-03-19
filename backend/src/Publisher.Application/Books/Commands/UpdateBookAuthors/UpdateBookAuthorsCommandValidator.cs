using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookAuthors;

public class UpdateBookAuthorsCommandValidator : AbstractValidator<UpdateBookAuthorsCommand>
{
    public UpdateBookAuthorsCommandValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required")
            .Must(Validation.IsValidGuid)
            .WithMessage("Book ID must be a valid GUID");

        RuleFor(c => c.AuthorIds)
            .NotEmpty()
            .WithMessage("At least one author is required")
            .Must(authorIds => authorIds.All(Validation.IsValidGuid))
            .WithMessage("Author IDs must be valid GUIDs");
    }
}