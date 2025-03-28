using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class BookRepository(AppDbContext _context) : IBookRepository
{
    public async Task<(List<Book>, int totalCount)> GetBooksPaginatedAsync(
        int page,
        int pageSize,
        string? title = null,
        string? author = null,
        string? genre = null,
        string? medium = null,
        int? year = null,
        CancellationToken token = default)
    {
        // Base query
        var query = _context.Books
            .Include(b => b.Covers)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .AsQueryable();

        // Apply filters that EF Core can translate to SQL - if provided
        if (!string.IsNullOrEmpty(title))
            query = query.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(author))
        query = query.Where(b => b.BookPersons.Any(bp => 
            bp.Author.FirstName.Contains(author, StringComparison.OrdinalIgnoreCase) ||
            bp.Author.LastName.Contains(author, StringComparison.OrdinalIgnoreCase) ||
            (bp.Author.FirstName + " " + bp.Author.LastName).Contains(author, StringComparison.OrdinalIgnoreCase)));
        
        if (year.HasValue)
            query = query.Where(b => b.PublishDate.Year == year.Value);

        // Materialize the query into memory after SQL-compatible filters are applied
        var books = await query.ToListAsync(token);

        // Apply filters on JSON collections (Genres and Mediums) in memory
        if (!string.IsNullOrEmpty(genre) && Enum.TryParse<Genre>(genre, true, out var genreEnum))
            books = books.Where(b => b.Genres.Contains(genreEnum)).ToList();

        if (!string.IsNullOrEmpty(medium) && Enum.TryParse<Medium>(medium, true, out var mediumEnum))
            books = books.Where(b => b.Mediums.Contains(mediumEnum)).ToList();

        // Get the total count of books
        var totalCount = books.Count;

        // Fetch the books for the current page
        var paginatedBooks = books
            .OrderBy(b => b.Title) // Order by title
            .Skip((page - 1) * pageSize) // Ensures only the requested page is returned
            .Take(pageSize) // Take only the requested number of records
            .ToList();

        return (paginatedBooks, totalCount);
    }
    public async Task<List<Book>> GetBooksAsync(CancellationToken token = default)
    {
        return await _context.Books.ToListAsync(token);
    }
    public async Task<Book?> GetBookByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .FirstOrDefaultAsync(b => b.BookId == id, token);
    }
    public async Task<Book?> GetBookBySlugAsync(string slug, CancellationToken token = default)
    {
        return await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .FirstOrDefaultAsync(b => b.Slug == slug, token);
    }
    public async Task<Book> CreateBookAsync(Book book, CancellationToken token = default)
    {
        await _context.Books.AddAsync(book, token);
        await _context.SaveChangesAsync(token);
        return book;
    }
    public async Task<Book?> UpdateBookAsync(Guid id, Book book, CancellationToken token = default)
    {
        var existingBook = await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                    .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(b => b.BookId == id, token);

        if (existingBook is null)
            return null!; // Let Application layer handle not found case

        _context.Entry(existingBook).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync(token);
        return existingBook;
    }
    public async Task<Book?> DeleteBookAsync(Guid id, CancellationToken token = default)
    {
        var book = await GetBookByIdAsync(id, token);

        if (book is null)
            return null;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(token);
        return book;
    }
    public async Task<bool> SlugExistsAsync(string slug, CancellationToken token = default)
    {
        return await _context.Books.AnyAsync(b => b.Slug == slug, token);
    }

    // Book Persons (Author) methods
    public async Task RemoveBookPersonsAsync(Guid bookId, CancellationToken token = default)
    {
        var bookPersons = await _context.BookPersons
            .Where(bp => bp.BookId == bookId)
            .ToListAsync();
        
        _context.BookPersons.RemoveRange(bookPersons);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task AddBookPersonsAsync(List<BookPersons> bookPersons, CancellationToken token = default)
    {
        await _context.BookPersons.AddRangeAsync(bookPersons, token);
        await _context.SaveChangesAsync(token);
    }
    
    // Book Covers methods
    public async Task RemoveBookCoversAsync(Guid bookId, CancellationToken token = default)
    {
        var covers = await _context.Covers
            .Where(c => c.BookId == bookId)
            .ToListAsync();
        
        // Also remove related CoverPersons
        foreach (var cover in covers)
        {
            await RemoveCoverPersonsAsync(cover.CoverId);
        }
        
        _context.Covers.RemoveRange(covers);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task AddBookCoverAsync(Cover cover, CancellationToken token = default)
    {
        await _context.Covers.AddAsync(cover, token);
        await _context.SaveChangesAsync(token);
    }
    
    // Cover Artist methods
    public async Task RemoveCoverPersonsAsync(Guid coverId, CancellationToken token = default)
    {
        var coverPersons = await _context.CoverPersons
            .Where(cp => cp.CoverId == coverId)
            .ToListAsync();
        
        _context.CoverPersons.RemoveRange(coverPersons);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task AddCoverPersonsAsync(List<CoverPersons> coverPersons, CancellationToken token = default)
    {
        await _context.CoverPersons.AddRangeAsync(coverPersons, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task UpdateBookAsync(Book book, CancellationToken token = default)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(token);
    }
}