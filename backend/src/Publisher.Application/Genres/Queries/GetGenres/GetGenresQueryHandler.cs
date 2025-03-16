using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(IGenreRepository _genreRepository)
    : IRequestHandler<GetGenresQuery, List<Genre>>
{
    public async Task<List<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        return await _genreRepository.GetGenresAsync();
    }
} 