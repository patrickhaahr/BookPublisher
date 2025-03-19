using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookCovers;

public class UpdateBookCoversCommandValidator : AbstractValidator<UpdateBookCoversCommand>
{
    public UpdateBookCoversCommandValidator()
    {
        RuleFor(c => c.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required")
            .Must(Validation.IsValidGuid)
            .WithMessage("Book ID must be a valid GUID");

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