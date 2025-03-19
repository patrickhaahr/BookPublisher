using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetGenresAsync(CancellationToken token = default);
    Task<Genre?> GetGenreByIdAsync(int id, CancellationToken token = default);
    Task<Genre> CreateGenreAsync(Genre genre, CancellationToken token = default);
    Task<Genre?> UpdateGenreAsync(int id, Genre genre, CancellationToken token = default);
    Task<Genre?> DeleteGenreAsync(int id, CancellationToken token = default);
} 