using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(IGenreRepository _genreRepository)
    : IRequestHandler<CreateGenreCommand, Genre>
{
    public async Task<Genre> Handle(CreateGenreCommand command, CancellationToken token)
    {
        var genre = new Genre
        {
            Name = command.Name
        };
        return await _genreRepository.CreateGenreAsync(genre);
    }
} 