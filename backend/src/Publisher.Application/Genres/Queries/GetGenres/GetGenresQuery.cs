using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Queries.GetGenres;

public record GetGenresQuery : IRequest<List<Genre>>; 