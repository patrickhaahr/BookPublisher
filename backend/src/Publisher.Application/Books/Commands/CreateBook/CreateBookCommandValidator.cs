using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IArtistRepository _artistRepository;

    public CreateBookCommandValidator(IGenreRepository genreRepository, IAuthorRepository authorRepository, IArtistRepository artistRepository)
    {
        _genreRepository = genreRepository;
        _authorRepository = authorRepository;
        _artistRepository = artistRepository;

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
            .WithMessage("Author IDs must be valid GUIDs")
            .MustAsync(async (authorIds, token) => 
                await Validation.AllEntitiesExistAsync(authorIds, _authorRepository.GetAuthorByIdAsync, token))
            .WithMessage("All author IDs must exist in the database");

        RuleFor(c => c.Covers)
            .NotEmpty()
            .WithMessage("At least one cover is required")
            .Must(covers => covers.All(c => Validation.IsValidBase64Image(c.ImgBase64)))
            .WithMessage("All covers must have a valid base64 image string")
            .Must(covers => covers.All(c => c.ArtistIds is not null && c.ArtistIds.Count > 0))
            .WithMessage("Each cover must have at least one artist")
            .Must(covers => covers.All(c => c.ArtistIds.All(Validation.IsValidGuid)))
            .WithMessage("Artist IDs must be valid GUIDs")
            .MustAsync(async (covers, token) => 
                await Validation.AllEntitiesExistAsync(covers.Select(c => c.ArtistIds.First()), _artistRepository.GetArtistByIdAsync, token))
            .WithMessage("All artist IDs must exist in the database");
    }
}
