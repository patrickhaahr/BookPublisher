using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IAuthorRepository
{
    Task<List<Author>> GetAuthorsAsync();
    Task<Author?> GetAuthorByIdAsync(Guid id);
    Task<Author> CreateAuthorAsync(Author author);
    Task<Author?> UpdateAuthorAsync(Guid id, Author author);
    Task<Author?> DeleteAuthorAsync(Guid id);
}

