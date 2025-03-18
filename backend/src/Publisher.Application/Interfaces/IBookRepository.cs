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
    Task RemoveBookGenresAsync(Guid bookId);
    Task AddBookGenresAsync(List<BookGenres> bookGenres);
    
    // Book Persons (Author) methods
    Task RemoveBookPersonsAsync(Guid bookId);
    Task AddBookPersonsAsync(List<BookPersons> bookPersons);
    
    // Book Covers methods
    Task RemoveBookCoversAsync(Guid bookId);
    Task AddBookCoverAsync(Cover cover);
    
    // Cover Artist methods
    Task RemoveCoverPersonsAsync(Guid coverId);
    Task AddCoverPersonsAsync(List<CoverPersons> coverPersons);

    Task UpdateBookAsync(Book book);
}

