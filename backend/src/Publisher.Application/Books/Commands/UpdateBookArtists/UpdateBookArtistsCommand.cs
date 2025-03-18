using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookArtists;

public record UpdateBookArtistsCommand(
    Guid BookId,
    List<Guid> ArtistIds
) : IRequest<UpdateBookArtistsResponse>; 