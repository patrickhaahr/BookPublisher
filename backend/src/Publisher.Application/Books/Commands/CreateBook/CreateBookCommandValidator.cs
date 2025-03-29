using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

namespace Publisher.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IArtistRepository _artistRepository;

    public CreateBookCommandValidator(IAuthorRepository authorRepository, IArtistRepository artistRepository)
    {
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
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base price must be greater than or equal to 0");
        
        RuleFor(c => c.Mediums)
            .NotEmpty()
            .WithMessage("At least one medium is required")
            .Must(mediums => mediums.All(m => Enum.TryParse<MediumEnum>(m, ignoreCase: true, out _)))
            .WithMessage("All mediums must be valid enum values (e.g., 'Print', 'EBook')");

        RuleFor(c => c.Genres)
            .NotEmpty()
            .WithMessage("At least one genre is required")
            .Must(genres => genres.All(g => Enum.TryParse<GenreEnum>(g, ignoreCase: true, out _)))
            .WithMessage("All genres must be valid enum values (e.g., 'ScienceFiction', 'Mystery')");

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
            .Must(covers => covers.All(c => c.ImgBase64.Length <= 1500000))
            .WithMessage("Cover image must be less than 1 MB")
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
