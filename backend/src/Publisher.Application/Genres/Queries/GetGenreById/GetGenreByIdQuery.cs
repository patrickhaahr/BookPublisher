using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Queries.GetGenreById;

public record GetGenreByIdQuery(int Id) : IRequest<Genre>; 