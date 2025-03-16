using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(int Id) : IRequest<Genre>; 