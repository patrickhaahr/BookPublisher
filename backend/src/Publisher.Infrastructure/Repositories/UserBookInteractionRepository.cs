using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class UserBookInteractionRepository(AppDbContext _context) : IUserBookInteractionRepository
{
    public async Task<List<UserBookInteraction>> GetUserBookInteractionsAsync()
    {
        return await _context.UserBookInteractions.ToListAsync();
    }

    public async Task<UserBookInteraction?> GetUserBookInteractionByIdAsync(Guid id)
    {
        return await _context.UserBookInteractions
            .Include(ubi => ubi.User)
            .Include(ubi => ubi.Book)
            .FirstOrDefaultAsync(ubi => ubi.InteractionId == id);
    }

    public async Task<UserBookInteraction> CreateUserBookInteractionAsync(UserBookInteraction interaction)
    {
        await _context.UserBookInteractions.AddAsync(interaction);
        await _context.SaveChangesAsync();
        return interaction;
    }

    public async Task<UserBookInteraction?> UpdateUserBookInteractionAsync(Guid id, UserBookInteraction interaction)
    {
        var existingInteraction = await _context.UserBookInteractions.FindAsync(id);
        
        if (existingInteraction is null)
            return null;

        _context.Entry(existingInteraction).CurrentValues.SetValues(interaction);
        await _context.SaveChangesAsync();
        return existingInteraction;
    }

    public async Task<UserBookInteraction?> DeleteUserBookInteractionAsync(Guid id)
    {
        var interaction = await GetUserBookInteractionByIdAsync(id);
        if (interaction is null)
            return null;

        _context.UserBookInteractions.Remove(interaction);
        await _context.SaveChangesAsync();
        return interaction;
    }
} 