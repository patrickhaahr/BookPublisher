using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class AuthorRepository(AppDbContext _context) : IAuthorRepository
{
    public async Task<List<Author>> GetAuthorsAsync()
    {
        return await _context.Authors.ToListAsync();
    }

    public async Task<Author?> GetAuthorByIdAsync(Guid id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> CreateAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author?> UpdateAuthorAsync(Guid id, Author author)
    {
        var existingAuthor = await _context.Authors.FindAsync(id);
        
        if (existingAuthor is null)
            return null;

        _context.Entry(existingAuthor).CurrentValues.SetValues(author);
        await _context.SaveChangesAsync();
        return existingAuthor;
    }

    public async Task<Author?> DeleteAuthorAsync(Guid id)
    {
        var Author = await GetAuthorByIdAsync(id);
        if (Author is null)
            return null;

        _context.Authors.Remove(Author);
        await _context.SaveChangesAsync();
        return Author;
    } 
}

