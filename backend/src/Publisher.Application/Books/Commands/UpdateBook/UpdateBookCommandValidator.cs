using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;
using Publisher.Domain.Enums;

namespace Publisher.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAuthorRepository _authorRepository;

    public UpdateBookCommandValidator(IArtistRepository artistRepository, IAuthorRepository authorRepository)
    {
        _artistRepository = artistRepository;
        _authorRepository = authorRepository;

        RuleFor(c => c.Title)
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");

        RuleFor(c => c.PublishDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publish date cannot be in the future");

        RuleFor(c => c.BasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base price must be greater than or equal to 0");

        RuleFor(c => c.Mediums)
            .Must(mediums => mediums is null || mediums.All(m => Enum.TryParse<MediumEnum>(m, ignoreCase: true, out _)))
            .WithMessage("All mediums must be valid enum values (e.g., 'Print', 'EBook')");
        
        RuleFor(c => c.Genres)
            .Must(genres => genres is null || genres.All(g => Enum.TryParse<GenreEnum>(g, ignoreCase: true, out _)))
            .WithMessage("All genres must be valid enum values (e.g., 'ScienceFiction', 'Mystery')");

        RuleFor(c => c.AuthorIds)
            .Must(authorIds => authorIds == null || authorIds.All(Validation.IsValidGuid))
            .WithMessage("Author IDs must be valid GUIDs.")
            .MustAsync(async (authorIds, token) => authorIds == null || await Validation.AllEntitiesExistAsync(authorIds, _authorRepository.GetAuthorByIdAsync, token))
            .When(c => c.AuthorIds != null)
            .WithMessage("All author IDs must exist in the database.");

        RuleForEach(c => c.Covers)
            .ChildRules(cover =>
            {
                cover.RuleFor(c => c.ImgBase64)
                    .Must(Validation.IsValidBase64Image)
                    .WithMessage("Cover image must be a valid base64 image string (max 1.5MB).");

                cover.RuleFor(c => c.ArtistIds)
                    .Must(artistIds => artistIds.All(Validation.IsValidGuid))
                    .WithMessage("Artist IDs must be valid GUIDs.")
                    .MustAsync(async (artistIds, token) => await Validation.AllEntitiesExistAsync(artistIds, _artistRepository.GetArtistByIdAsync, token))
                    .WithMessage("All artist IDs must exist in the database.");
            });
    }
}