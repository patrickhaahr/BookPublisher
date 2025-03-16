using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class CoverRepository(AppDbContext _context) : ICoverRepository
{
    public async Task<List<Cover>> GetCoversAsync()
    {
        return await _context.Covers.ToListAsync();
    }

    public async Task<Cover?> GetCoverByIdAsync(Guid id)
    {
        return await _context.Covers
            .Include(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(c => c.CoverId == id);
    }

    public async Task<Cover> CreateCoverAsync(Cover cover)
    {
        await _context.Covers.AddAsync(cover);
        await _context.SaveChangesAsync();
        return cover;
    }

    public async Task<Cover?> UpdateCoverAsync(Guid id, Cover cover)
    {
        var existingCover = await _context.Covers
            .Include(c => c.CoverPersons)
                .ThenInclude(cp => cp.Artist)
            .FirstOrDefaultAsync(c => c.CoverId == id);
        
        if (existingCover is null)
            return null;

        _context.Entry(existingCover).CurrentValues.SetValues(cover);
        await _context.SaveChangesAsync();
        return existingCover;
    }

    public async Task<Cover?> DeleteCoverAsync(Guid id)
    {
        var cover = await GetCoverByIdAsync(id);
        if (cover is null)
            return null;

        _context.Covers.Remove(cover);
        await _context.SaveChangesAsync();
        return cover;
    }
} 