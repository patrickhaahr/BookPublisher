using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class BookRepository(AppDbContext _context) : IBookRepository
{
    public async Task<List<Book>> GetBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }
    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.BookId == id);
    }
    public async Task<Book> CreateBookAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task<Book?> UpdateBookAsync(Guid id, Book book)
    {
        var existingBook = await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                    .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(b => b.BookId == id);

        if (existingBook is null)
            return null!; // Let Application layer handle not found case

        _context.Entry(existingBook).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();
        return existingBook;
    }
    public async Task<Book?> DeleteBookAsync(Guid id)
    {
        var book = await GetBookByIdAsync(id);
        if (book is null)
            return null;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task<bool> SlugExistsAsync(string slug)
    {
        return await _context.Books.AnyAsync(b => b.Slug == slug);
    }
    public async Task RemoveBookGenresAsync(Guid bookId)
    {
        var genres = await _context.BookGenres
            .Where(bg => bg.BookId == bookId)
            .ToListAsync();
        
        _context.BookGenres.RemoveRange(genres);
        await _context.SaveChangesAsync();
    }
    public async Task AddBookGenresAsync(List<BookGenres> bookGenres)
    {
        await _context.BookGenres.AddRangeAsync(bookGenres);
        await _context.SaveChangesAsync();
    }
}

