using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookGenres;

public class UpdateBookGenresCommandValidator : AbstractValidator<UpdateBookGenresCommand>
{
    public UpdateBookGenresCommandValidator()
    {
        RuleFor(c => c.GenreIds)
            .NotEmpty()
            .WithMessage("At least one genre is required")
            .Must(genreIds => genreIds.All(g => g > 0))
            .WithMessage("Genre IDs must be positive integers");
    }
}