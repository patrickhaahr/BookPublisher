using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Enums;

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
        string? year = null,
        CancellationToken token = default)
    {
        // Start with the base query that includes all necessary related entities
        var query = BookRepositoryCompiledQueries.GetBooksWithIncludes(_context);
        
        // Apply filters
        if (!string.IsNullOrEmpty(title))
            query = query.Where(b => EF.Functions.Like(b.Title, $"%{title}%")); // Case-insensitive LIKE

        if (!string.IsNullOrEmpty(author))
            query = query.Where(b => b.BookPersons.Any(bp =>
                EF.Functions.Like(bp.Author.FirstName, $"%{author}%") ||
                EF.Functions.Like(bp.Author.LastName, $"%{author}%") ||
                EF.Functions.Like(bp.Author.FirstName + " " + bp.Author.LastName, $"%{author}%")));

        if (!string.IsNullOrEmpty(genre) && Enum.TryParse<GenreEnum>(genre, true, out var genreEnum))
            query = query.Where(b => b.BookGenres.Any(bg => bg.GenreId == (int)genreEnum));

        if (!string.IsNullOrEmpty(medium) && Enum.TryParse<MediumEnum>(medium, true, out var mediumEnum))
            query = query.Where(b => b.BookMediums.Any(bm => bm.MediumId == (int)mediumEnum));
        
        if (!string.IsNullOrEmpty(year))
            query = query.Where(b => EF.Functions.Like(b.PublishDate.Year.ToString(), $"{year}%"));

        // Order the query
        query = query.OrderBy(b => b.Title);
        
        // Get the total count of books using our compiled count function
        var totalCount = await BookRepositoryCompiledQueries.CountBooksAsync(
            _context, title, author, genre, medium, year);

        // Apply paging and get the final results
        var pagedQuery = BookRepositoryCompiledQueries.ApplyPaging(query, (page - 1) * pageSize, pageSize);
        var books = await pagedQuery.ToListAsync(token);

        return (books, totalCount);
    }
    public async Task<List<Book>> GetBooksAsync(CancellationToken token = default)
    {
        return await _context.Books
            .Include(b => b.Covers)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .Include(b => b.BookMediums)
                .ThenInclude(bm => bm.Medium)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .ToListAsync(token);
    }
    public async Task<Book?> GetBookByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Books
            .Include(b => b.Covers)
                .ThenInclude(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .Include(b => b.BookMediums)
                .ThenInclude(bm => bm.Medium)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
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
            .Include(b => b.BookMediums)
                .ThenInclude(bm => bm.Medium)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
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
            .Include(b => b.BookPersons)
                .ThenInclude(bp => bp.Author)
            .Include(b => b.BookMediums)
                .ThenInclude(bm => bm.Medium)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
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