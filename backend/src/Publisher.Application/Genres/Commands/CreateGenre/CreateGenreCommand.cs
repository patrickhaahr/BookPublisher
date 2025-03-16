using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Commands.CreateGenre;

public record CreateGenreCommand(
    string Name) : IRequest<Genre>; 