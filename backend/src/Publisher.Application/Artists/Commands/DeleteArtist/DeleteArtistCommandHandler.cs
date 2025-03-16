using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Artists.Commands.DeleteArtist;

public class DeleteArtistCommandHandler(IArtistRepository _artistRepository)
    : IRequestHandler<DeleteArtistCommand, Artist>
{
    public async Task<Artist> Handle(DeleteArtistCommand command, CancellationToken token)
    {
        var deletedArtist = await _artistRepository.DeleteArtistAsync(command.Id);

        return deletedArtist is null
            ? throw new NotFoundException(nameof(Artist), command.Id)
            : deletedArtist;
    }
} 