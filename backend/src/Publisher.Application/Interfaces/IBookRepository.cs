using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetBooksAsync(CancellationToken token = default);
    Task<(List<Book>, int totalCount)> GetBooksPaginatedAsync(
        int page,
        int pageSize,
        string? title = null,
        string? author = null,
        string? genre = null,
        string? medium = null,
        string? year = null,
        CancellationToken token = default);
    Task<Book?> GetBookByIdAsync(Guid id, CancellationToken token = default);
    Task<Book?> GetBookBySlugAsync(string slug, CancellationToken token = default);
    Task<Book> CreateBookAsync(Book book, CancellationToken token = default);
    Task<Book?> DeleteBookAsync(Guid id, CancellationToken token = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken token = default);

    // Book Persons (Author) methods
    Task RemoveBookPersonsAsync(Guid bookId, CancellationToken token = default);
    Task AddBookPersonsAsync(List<BookPersons> bookPersons, CancellationToken token = default);
    
    // Book Covers methods
    Task RemoveBookCoversAsync(Guid bookId, CancellationToken token = default);
    Task AddBookCoverAsync(Cover cover, CancellationToken token = default);
    
    // Cover Artist methods
    Task RemoveCoverPersonsAsync(Guid coverId, CancellationToken token = default);
    Task AddCoverPersonsAsync(List<CoverPersons> coverPersons, CancellationToken token = default);

    Task UpdateBookAsync(Book book, CancellationToken token = default);
}

