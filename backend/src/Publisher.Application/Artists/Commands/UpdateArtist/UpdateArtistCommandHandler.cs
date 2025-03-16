using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Artists.Commands.UpdateArtist;

public class UpdateArtistCommandHandler(IArtistRepository _artistRepository)
    : IRequestHandler<UpdateArtistCommand, Artist>
{
    public async Task<Artist> Handle(UpdateArtistCommand command, CancellationToken token)
    {
        var artist = await _artistRepository.GetArtistByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Artist), command.Id);

        artist.FirstName = command.FirstName;
        artist.LastName = command.LastName;
        artist.Email = command.Email;
        artist.Phone = command.Phone;
        artist.PortfolioUrl = command.PortfolioUrl;

        var updatedArtist = await _artistRepository.UpdateArtistAsync(command.Id, artist);

        return updatedArtist is null
            ? throw new NotFoundException(nameof(Artist), command.Id)
            : updatedArtist;
    }
} 