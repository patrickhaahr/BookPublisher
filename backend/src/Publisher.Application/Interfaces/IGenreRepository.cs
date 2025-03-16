using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetGenresAsync();
    Task<Genre?> GetGenreByIdAsync(int id);
    Task<Genre> CreateGenreAsync(Genre genre);
    Task<Genre?> UpdateGenreAsync(int id, Genre genre);
    Task<Genre?> DeleteGenreAsync(int id);
} 