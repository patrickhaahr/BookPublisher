using Publisher.Domain.Entities;

namespace Publisher.Application.Interfaces;

public interface IUserBookInteractionRepository
{
    Task<List<UserBookInteraction>> GetUserBookInteractionsAsync();
    Task<UserBookInteraction?> GetUserBookInteractionByIdAsync(Guid id);
    Task<UserBookInteraction> CreateUserBookInteractionAsync(UserBookInteraction interaction);
    Task<UserBookInteraction?> UpdateUserBookInteractionAsync(Guid id, UserBookInteraction interaction);
    Task<UserBookInteraction?> DeleteUserBookInteractionAsync(Guid id);
} 