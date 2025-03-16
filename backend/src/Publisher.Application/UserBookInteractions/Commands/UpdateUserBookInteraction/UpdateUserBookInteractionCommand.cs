using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;

public record UpdateUserBookInteractionCommand(
    Guid Id,
    Guid UserId,
    Guid BookId,
    bool IsFavorite,
    bool IsSaved,
    string? Status,
    int? Rating) : IRequest<UserBookInteraction>; 