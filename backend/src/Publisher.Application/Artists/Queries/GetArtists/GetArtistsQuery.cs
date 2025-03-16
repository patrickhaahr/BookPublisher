using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Artists.Queries.GetArtists;

public record GetArtistsQuery : IRequest<List<Artist>>; 