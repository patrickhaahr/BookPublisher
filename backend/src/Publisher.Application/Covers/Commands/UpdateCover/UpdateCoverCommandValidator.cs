using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.Covers.Commands.UpdateCover;

public class UpdateCoverCommandValidator : AbstractValidator<UpdateCoverCommand>
{
    private readonly IArtistRepository _artistRepository;

    public UpdateCoverCommandValidator(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;

        RuleFor(c => c.ImgBase64)
            .NotEmpty()
            .WithMessage("Image base64 is required")
            .Must(Validation.IsValidBase64Image)
            .WithMessage("Image base64 must be a valid base64 string");
        
        // RuleFor(a => a.ArtistIds)
        //     .NotEmpty()
        //     .WithMessage("Artist IDs are required")
        //     .Must(ids => ids.All(Validation.IsValidGuid))
        //     .WithMessage("Artist IDs must be valid GUIDs")
        //     .Must(ids => ids.All(async id => await _artistRepository.GetArtistByIdAsync(id) is not null))
        //     .WithMessage("All artist IDs must exist in the database");
    }
}

