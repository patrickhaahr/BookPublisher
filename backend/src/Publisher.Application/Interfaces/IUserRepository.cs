using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync(CancellationToken token = default);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default);
    Task<User> CreateUserAsync(User user, CancellationToken token = default);
    Task<User?> UpdateUserAsync(Guid id, User user, CancellationToken token = default);
    Task<User?> DeleteUserAsync(Guid id, CancellationToken token = default);

    Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default);
} 