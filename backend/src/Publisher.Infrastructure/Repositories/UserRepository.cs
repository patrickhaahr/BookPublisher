using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.UserBookInteractions)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUserAsync(Guid id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        
        if (existingUser is null)
            return null;

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User?> DeleteUserAsync(Guid id)
    {
        var user = await GetUserByIdAsync(id);
        if (user is null)
            return null;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
} 