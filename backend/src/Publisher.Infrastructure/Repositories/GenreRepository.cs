using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class GenreRepository(AppDbContext _context) : IGenreRepository
{
    public async Task<List<Genre>> GetGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre?> GetGenreByIdAsync(int id)
    {
        return await _context.Genres
            .Include(g => g.BookGenres)
            .FirstOrDefaultAsync(g => g.GenreId == id);
    }

    public async Task<Genre> CreateGenreAsync(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre?> UpdateGenreAsync(int id, Genre genre)
    {
        var existingGenre = await _context.Genres.FindAsync(id);
        
        if (existingGenre is null)
            return null;

        _context.Entry(existingGenre).CurrentValues.SetValues(genre);
        await _context.SaveChangesAsync();
        return existingGenre;
    }

    public async Task<Genre?> DeleteGenreAsync(int id)
    {
        var genre = await GetGenreByIdAsync(id);
        if (genre is null)
            return null;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return genre;
    }
} 