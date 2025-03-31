using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class AuthorRepository(AppDbContext _context) : IAuthorRepository
{
    public async Task<(List<Author> Items, int TotalCount, int TotalPages)> GetAuthorsAsync(int page, int pageSize, CancellationToken token = default)
    {
        // Ensure valid pagination parameters
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        // Calculate total count and total pages
        int totalCount = await _context.Authors.CountAsync(token);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Get paginated data
        var items = await _context.Authors
            .OrderBy(a => a.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (items, totalCount, totalPages);
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Authors.FindAsync(id, token);
    }

    public async Task<Author> CreateAuthorAsync(Author author, CancellationToken token = default)
    {
        await _context.Authors.AddAsync(author, token);
        await _context.SaveChangesAsync(token);
        return author;
    }

    public async Task<Author?> UpdateAuthorAsync(Guid id, Author author, CancellationToken token = default)
    {
        var existingAuthor = await _context.Authors.FindAsync(id, token);
        
        if (existingAuthor is null)
            return null;

        _context.Entry(existingAuthor).CurrentValues.SetValues(author);
        await _context.SaveChangesAsync();
        return existingAuthor;
    }

    public async Task<Author?> DeleteAuthorAsync(Guid id, CancellationToken token = default)
    {
        var Author = await GetAuthorByIdAsync(id, token);
        if (Author is null)
            return null;

        _context.Authors.Remove(Author);
        await _context.SaveChangesAsync(token);
        return Author;
    } 
}

