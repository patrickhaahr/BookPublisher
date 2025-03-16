using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(Guid id, User user);
    Task<User?> DeleteUserAsync(Guid id);
} 