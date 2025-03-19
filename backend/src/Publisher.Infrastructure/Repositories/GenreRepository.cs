using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class GenreRepository(AppDbContext _context) : IGenreRepository
{
    public async Task<List<Genre>> GetGenresAsync(CancellationToken token = default)
    {
        return await _context.Genres.ToListAsync(token);
    }

    public async Task<Genre?> GetGenreByIdAsync(int id, CancellationToken token = default)
    {
        return await _context.Genres
            .Include(g => g.BookGenres)
            .FirstOrDefaultAsync(g => g.GenreId == id, token);
    }

    public async Task<Genre> CreateGenreAsync(Genre genre, CancellationToken token = default)
    {
        await _context.Genres.AddAsync(genre, token);
        await _context.SaveChangesAsync(token);
        return genre;
    }

    public async Task<Genre?> UpdateGenreAsync(int id, Genre genre, CancellationToken token = default)
    {
        var existingGenre = await _context.Genres.FindAsync(id, token);
        
        if (existingGenre is null)
            return null;

        _context.Entry(existingGenre).CurrentValues.SetValues(genre);
        await _context.SaveChangesAsync(token);
        return existingGenre;
    }

    public async Task<Genre?> DeleteGenreAsync(int id, CancellationToken token = default)
    {
        var genre = await GetGenreByIdAsync(id, token);
        if (genre is null)
            return null;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync(token);
        return genre;
    }
} 