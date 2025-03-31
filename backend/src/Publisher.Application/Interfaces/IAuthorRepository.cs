using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IAuthorRepository
{
    Task<(List<Author> Items, int TotalCount, int TotalPages)> GetAuthorsAsync(int page, int pageSize, CancellationToken token = default);
    Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken token = default);
    Task<Author> CreateAuthorAsync(Author author, CancellationToken token = default);
    Task<Author?> UpdateAuthorAsync(Guid id, Author author, CancellationToken token = default);
    Task<Author?> DeleteAuthorAsync(Guid id, CancellationToken token = default);
}

