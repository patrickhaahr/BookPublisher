using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler(IGenreRepository _genreRepository)
    : IRequestHandler<DeleteGenreCommand, Genre>
{
    public async Task<Genre> Handle(DeleteGenreCommand command, CancellationToken token)
    {
        var deletedGenre = await _genreRepository.DeleteGenreAsync(command.Id);

        return deletedGenre is null
            ? throw new NotFoundException(nameof(Genre), command.Id)
            : deletedGenre;
    }
} 