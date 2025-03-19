using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
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

        RuleFor(c => c.GenreIds)
            .NotEmpty()
            .WithMessage("At least one genre is required")
            .Must(genreIds => genreIds.All(g => g > 0))
            .WithMessage("Genre IDs must be positive integers");

        RuleFor(c => c.AuthorIds)
            .NotEmpty()
            .WithMessage("At least one author is required")
            .Must(authorIds => authorIds.All(Validation.IsValidGuid))
            .WithMessage("Author IDs must be valid GUIDs");

        RuleFor(c => c.Covers)
            .NotEmpty()
            .WithMessage("At least one cover is required")
            .Must(covers => covers.All(c => !string.IsNullOrEmpty(c.ImgBase64)))
            .WithMessage("All covers must have a valid base64 image string")
            .Must(covers => covers.All(c => c.ArtistIds is not null && c.ArtistIds.Count > 0))
            .WithMessage("Each cover must have at least one artist")
            .Must(covers => covers.All(c => c.ArtistIds.All(Validation.IsValidGuid)))
            .WithMessage("Artist IDs must be valid GUIDs");
    }
}
