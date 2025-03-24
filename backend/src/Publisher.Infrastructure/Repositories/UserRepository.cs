using Microsoft.EntityFrameworkCore;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Infrastructure.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<List<User>> GetUsersAsync(CancellationToken token = default)
    {
        return await _context.Users.ToListAsync(token);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _context.Users
            .Include(u => u.UserBookInteractions)
            .FirstOrDefaultAsync(u => u.UserId == id, token);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken token = default)
    {
        await _context.Users.AddAsync(user, token);
        await _context.SaveChangesAsync(token);
        return user;
    }

    public async Task<User?> UpdateUserAsync(Guid id, User user, CancellationToken token = default)
    {
        var existingUser = await _context.Users.FindAsync(id, token);
        
        if (existingUser is null)
            return null;

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync(token);
        return existingUser;
    }

    public async Task<User?> DeleteUserAsync(Guid id, CancellationToken token = default)
    {
        var user = await GetUserByIdAsync(id, token);
        if (user is null)
            return null;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(token);
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, token);
    }
} 