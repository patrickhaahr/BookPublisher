using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;

public record CreateUserBookInteractionCommand(
    Guid UserId,
    Guid BookId,
    bool IsFavorite,
    bool IsSaved,
    string? Status,
    int? Rating) : IRequest<UserBookInteraction>; 