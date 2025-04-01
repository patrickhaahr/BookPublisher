using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IUserBookInteractionRepository
{
    Task<List<UserBookInteraction>> GetUserBookInteractionsAsync(CancellationToken token = default);
    Task<List<UserBookInteraction>> GetUserBookInteractionsByUserIdAsync(Guid userId, CancellationToken token = default);
    Task<UserBookInteraction?> GetUserBookInteractionByIdAsync(Guid id, CancellationToken token = default);
    Task<UserBookInteraction?> GetUserBookInteractionByUserAndBookAsync(Guid userId, Guid bookId, CancellationToken token = default);
    Task<UserBookInteraction> CreateUserBookInteractionAsync(UserBookInteraction interaction, CancellationToken token = default);
    Task<UserBookInteraction?> UpdateUserBookInteractionAsync(Guid id, UserBookInteraction interaction, CancellationToken token = default);
    Task<UserBookInteraction?> DeleteUserBookInteractionAsync(Guid id, CancellationToken token = default);
} 