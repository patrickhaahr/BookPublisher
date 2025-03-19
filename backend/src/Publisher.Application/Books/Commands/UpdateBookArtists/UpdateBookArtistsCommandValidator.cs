using FluentValidation;
using Publisher.Application.Interfaces;
using Publisher.Application.Utils;

namespace Publisher.Application.Books.Commands.UpdateBookArtists;

public class UpdateBookArtistsCommandValidator : AbstractValidator<UpdateBookArtistsCommand>
{
    private readonly IArtistRepository _artistRepository;

    public UpdateBookArtistsCommandValidator(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;

        RuleFor(c => c.ArtistIds)
            .NotEmpty()
            .WithMessage("At least one artist is required")
            .Must(artistIds => artistIds.All(Validation.IsValidGuid))
            .WithMessage("Artist IDs must be valid GUIDs")
            .MustAsync(async (artistIds, token) => 
                await Validation.AllEntitiesExistAsync(artistIds, _artistRepository.GetArtistByIdAsync, token))
            .WithMessage("All artist IDs must exist in the database");
    }
}   
