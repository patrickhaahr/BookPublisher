using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class ArtistRepository(AppDbContext _context) : IArtistRepository
{
    public async Task<(List<Artist> Items, int TotalCount, int TotalPages)> GetArtistsAsync(int page, int pageSize, CancellationToken token = default)
    {
        // Ensure valid pagination parameters
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        // Calculate total count and total pages
        int totalCount = await _context.Artists.CountAsync(token);
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Get paginated data
        var items = await _context.Artists
            .OrderBy(a => a.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (items, totalCount, totalPages);
    }

    public async Task<Artist?> GetArtistByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Artists.FindAsync(id, token);
    }

    public async Task<Artist> CreateArtistAsync(Artist artist, CancellationToken token = default)
    {
        await _context.Artists.AddAsync(artist, token);
        await _context.SaveChangesAsync(token);
        return artist;
    }

    public async Task<Artist?> UpdateArtistAsync(Guid id, Artist artist, CancellationToken token = default)
    {
        var existingArtist = await _context.Artists.FindAsync(id, token);
        
        if (existingArtist is null)
            return null;

        _context.Entry(existingArtist).CurrentValues.SetValues(artist);
        await _context.SaveChangesAsync();
        return existingArtist;
    }

    public async Task<Artist?> DeleteArtistAsync(Guid id, CancellationToken token = default)
    {
        var artist = await GetArtistByIdAsync(id, token);
        if (artist is null)
            return null;

        _context.Artists.Remove(artist);
        await _context.SaveChangesAsync();
        return artist;
    }
} 