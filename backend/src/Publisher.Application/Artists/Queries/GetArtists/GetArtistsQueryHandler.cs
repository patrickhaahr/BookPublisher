using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Queries.GetArtists;

public class GetArtistsQueryHandler(IArtistRepository _artistRepository)
    : IRequestHandler<GetArtistsQuery, List<Artist>>
{
    public async Task<List<Artist>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
    {
        return await _artistRepository.GetArtistsAsync();
    }
} 