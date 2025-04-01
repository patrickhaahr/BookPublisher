using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class UserBookInteractionRepository(AppDbContext _context) : IUserBookInteractionRepository
{
    public async Task<List<UserBookInteraction>> GetUserBookInteractionsAsync(CancellationToken token = default)
    {
        return await _context.UserBookInteractions.ToListAsync(token);
    }

    public async Task<List<UserBookInteraction>> GetUserBookInteractionsByUserIdAsync(Guid userId, CancellationToken token = default)
    {
        return await _context.UserBookInteractions
            .Include(ubi => ubi.User)
            .Include(ubi => ubi.Book)
            .Where(ubi => ubi.UserId == userId)
            .ToListAsync(token);
    }

    public async Task<UserBookInteraction?> GetUserBookInteractionByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.UserBookInteractions
            .Include(ubi => ubi.User)
            .Include(ubi => ubi.Book)
            .FirstOrDefaultAsync(ubi => ubi.InteractionId == id, token);
    }

    public async Task<UserBookInteraction?> GetUserBookInteractionByUserAndBookAsync(Guid userId, Guid bookId, CancellationToken token = default)
    {
        return await _context.UserBookInteractions
            .Include(ubi => ubi.User)
            .Include(ubi => ubi.Book)
            .FirstOrDefaultAsync(ubi => ubi.UserId == userId && ubi.BookId == bookId, token);
    }

    public async Task<UserBookInteraction> CreateUserBookInteractionAsync(UserBookInteraction interaction, CancellationToken token = default)
    {
        await _context.UserBookInteractions.AddAsync(interaction, token);
        await _context.SaveChangesAsync(token);
        return interaction;
    }

    public async Task<UserBookInteraction?> UpdateUserBookInteractionAsync(Guid id, UserBookInteraction interaction, CancellationToken token = default)
    {
        var existingInteraction = await _context.UserBookInteractions.FindAsync(id, token);
        
        if (existingInteraction is null)
            return null;

        _context.Entry(existingInteraction).CurrentValues.SetValues(interaction);
        await _context.SaveChangesAsync(token);
        return existingInteraction;
    }

    public async Task<UserBookInteraction?> DeleteUserBookInteractionAsync(Guid id, CancellationToken token = default)
    {
        var interaction = await GetUserBookInteractionByIdAsync(id, token);
        if (interaction is null)
            return null;

        _context.UserBookInteractions.Remove(interaction);
        await _context.SaveChangesAsync(token);
        return interaction;
    }
} 