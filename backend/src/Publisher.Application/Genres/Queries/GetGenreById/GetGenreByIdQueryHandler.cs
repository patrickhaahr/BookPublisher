using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler(IGenreRepository _genreRepository)
    : IRequestHandler<GetGenreByIdQuery, Genre>
{
    public async Task<Genre> Handle(GetGenreByIdQuery query, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetGenreByIdAsync(query.Id);
        
        return genre is null
            ? throw new NotFoundException(nameof(Genre), query.Id)
            : genre;
    }
} 