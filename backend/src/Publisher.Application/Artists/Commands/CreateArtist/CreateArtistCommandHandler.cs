using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Commands.CreateArtist;

public class CreateArtistCommandHandler(IArtistRepository _artistRepository)
    : IRequestHandler<CreateArtistCommand, ArtistResponse>
{
    public async Task<ArtistResponse> Handle(CreateArtistCommand command, CancellationToken token)
    {
        var artist = new Artist
        {
            PersonId = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Phone = command.Phone,
            PortfolioUrl = command.PortfolioUrl
        };
        await _artistRepository.CreateArtistAsync(artist, token);

        return new ArtistResponse(
            artist.PersonId,
            artist.FirstName,
            artist.LastName,
            artist.Email,
            artist.Phone ?? string.Empty,
            artist.PortfolioUrl ?? string.Empty
        );
    }
} 