using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class ArtistRepository(AppDbContext _context) : IArtistRepository
{
    public async Task<List<Artist>> GetArtistsAsync()
    {
        return await _context.Artists.ToListAsync();
    }

    public async Task<Artist?> GetArtistByIdAsync(Guid id)
    {
        return await _context.Artists.FindAsync(id);
    }

    public async Task<Artist> CreateArtistAsync(Artist artist)
    {
        await _context.Artists.AddAsync(artist);
        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task<Artist?> UpdateArtistAsync(Guid id, Artist artist)
    {
        var existingArtist = await _context.Artists.FindAsync(id);
        
        if (existingArtist is null)
            return null;

        _context.Entry(existingArtist).CurrentValues.SetValues(artist);
        await _context.SaveChangesAsync();
        return existingArtist;
    }

    public async Task<Artist?> DeleteArtistAsync(Guid id)
    {
        var artist = await GetArtistByIdAsync(id);
        if (artist is null)
            return null;

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
        return artist;
    }
} 