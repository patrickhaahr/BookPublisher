using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Artists.Queries.GetArtists;

public class GetArtistsQueryHandler(IArtistRepository _artistRepository)
    : IRequestHandler<GetArtistsQuery, PaginatedResponse<ArtistResponse>>
{
    public async Task<PaginatedResponse<ArtistResponse>> Handle(GetArtistsQuery request, CancellationToken token)
    {
        var (artists, totalCount, totalPages) = await _artistRepository.GetArtistsAsync(
            request.Page,
            request.PageSize,
            token);

        var items = artists.Select(artist => new ArtistResponse(
            artist.PersonId,
            artist.FirstName,
            artist.LastName,
            artist.Email,
            artist.Phone ?? string.Empty,
            artist.PortfolioUrl ?? string.Empty
        )).ToList();

        bool hasNextPage = request.Page < totalPages;
        bool hasPrevPage = request.Page > 1;

        return new PaginatedResponse<ArtistResponse>(
            items,
            request.Page,
            request.PageSize,
            totalCount,
            totalPages,
            hasNextPage,
            hasPrevPage
        );
    }
} 