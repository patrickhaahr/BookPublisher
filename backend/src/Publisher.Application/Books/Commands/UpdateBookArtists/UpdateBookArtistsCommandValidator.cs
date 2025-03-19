using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookArtists;

public class UpdateBookArtistsCommandValidator : AbstractValidator<UpdateBookArtistsCommand>
{
    public UpdateBookArtistsCommandValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required")
            .Must(Validation.IsValidGuid)
            .WithMessage("Book ID must be a valid GUID");

        RuleFor(c => c.ArtistIds)
            .NotEmpty()
            .WithMessage("At least one artist is required")
            .Must(artistIds => artistIds.All(Validation.IsValidGuid))
            .WithMessage("Artist IDs must be valid GUIDs");
    }
}   
