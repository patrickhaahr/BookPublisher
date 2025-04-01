using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;

public record CreateUserBookInteractionCommand(
    Guid UserId,
    object BookId,
    bool? IsFavorite = false,
    bool? IsSaved = false,
    string? Status = null,
    int? Rating = null) : IRequest<UserBookInteractionResponse>; 