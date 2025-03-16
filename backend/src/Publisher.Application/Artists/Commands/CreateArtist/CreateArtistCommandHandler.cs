using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Commands.CreateArtist;

public class CreateArtistCommandHandler(IArtistRepository _artistRepository)
    : IRequestHandler<CreateArtistCommand, Artist>
{
    public async Task<Artist> Handle(CreateArtistCommand command, CancellationToken token)
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
        return await _artistRepository.CreateArtistAsync(artist);
    }
} 