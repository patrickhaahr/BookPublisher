using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(Guid id);
    Task<Book> CreateBookAsync(Book book);
    Task<Book?> UpdateBookAsync(Guid id, Book book);
    Task<Book?> DeleteBookAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug);
}

