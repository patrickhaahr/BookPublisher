using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler(IGenreRepository _genreRepository)
    : IRequestHandler<UpdateGenreCommand, Genre>
{
    public async Task<Genre> Handle(UpdateGenreCommand command, CancellationToken token)
    {
        var genre = await _genreRepository.GetGenreByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Genre), command.Id);

        genre.Name = command.Name;

        var updatedGenre = await _genreRepository.UpdateGenreAsync(command.Id, genre);

        return updatedGenre is null
            ? throw new NotFoundException(nameof(Genre), command.Id)
            : updatedGenre;
    }
} 