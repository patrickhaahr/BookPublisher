using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Queries.GetArtistById;

public record GetArtistByIdQuery(Guid Id) : IRequest<Artist>; 