using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(
    int Id,
    string Name) : IRequest<Genre>; 