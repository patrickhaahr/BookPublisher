using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class CoverRepository(AppDbContext _context) : ICoverRepository
{
    public async Task<List<Cover>> GetCoversAsync(CancellationToken token = default)
    {
        return await _context.Covers.ToListAsync(token);
    }

    public async Task<Cover?> GetCoverByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Covers
            .Include(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(c => c.CoverId == id, token);
    }

    public async Task<Cover> CreateCoverAsync(Cover cover, CancellationToken token = default)
    {
        await _context.Covers.AddAsync(cover, token);
        await _context.SaveChangesAsync(token);
        return cover;
    }

    public async Task<Cover?> UpdateCoverAsync(Guid id, Cover cover, CancellationToken token = default)
    {
        var existingCover = await _context.Covers
            .Include(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(c => c.CoverId == id, token);
        
        if (existingCover is null)
            return null;

        _context.Entry(existingCover).CurrentValues.SetValues(cover);
        await _context.SaveChangesAsync();
        return existingCover;
    }

    public async Task<Cover?> DeleteCoverAsync(Guid id, CancellationToken token = default)
    {
        var cover = await GetCoverByIdAsync(id, token);
        if (cover is null)
            return null;

        _context.Covers.Remove(cover);
        await _context.SaveChangesAsync(token);
        return cover;
    }
} 