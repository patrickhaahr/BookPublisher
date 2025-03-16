using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Artists.Queries.GetArtistById;

public class GetArtistByIdQueryHandler(IArtistRepository _artistRepository)
    : IRequestHandler<GetArtistByIdQuery, Artist>
{
    public async Task<Artist> Handle(GetArtistByIdQuery query, CancellationToken cancellationToken)
    {
        var artist = await _artistRepository.GetArtistByIdAsync(query.Id);
        
        return artist is null
            ? throw new NotFoundException(nameof(Artist), query.Id)
            : artist;
    }
} 