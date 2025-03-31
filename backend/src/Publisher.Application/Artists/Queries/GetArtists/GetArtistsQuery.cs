using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Artists.Queries.GetArtists;

public record GetArtistsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchQuery = null
) : IRequest<PaginatedResponse<ArtistResponse>>; 