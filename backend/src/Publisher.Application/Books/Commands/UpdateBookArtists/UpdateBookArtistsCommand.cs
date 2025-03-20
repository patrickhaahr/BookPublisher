using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookArtists;

public record UpdateBookArtistsCommand(
    string IdOrSlug,
    List<Guid> ArtistIds
) : IRequest<UpdateBookArtistsResponse>; 