using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class AuthorRepository(AppDbContext _context) : IAuthorRepository
{
    public async Task<List<Author>> GetAuthorsAsync(CancellationToken token = default)
    {
        return await _context.Authors.ToListAsync(token);
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

