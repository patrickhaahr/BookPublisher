using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookGenres;

public class UpdateBookGenresCommandValidator : AbstractValidator<UpdateBookGenresCommand>
{
    public UpdateBookGenresCommandValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required")
            .Must(Validation.IsValidGuid)
            .WithMessage("Book ID must be a valid GUID");

        RuleFor(c => c.GenreIds)
            .NotEmpty()
            .WithMessage("At least one genre is required")
            .Must(genreIds => genreIds.All(g => g > 0))
            .WithMessage("Genre IDs must be positive integers");
    }
}