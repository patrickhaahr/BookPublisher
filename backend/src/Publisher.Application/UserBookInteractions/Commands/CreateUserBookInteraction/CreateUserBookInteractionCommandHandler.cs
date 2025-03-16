using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;

public class CreateUserBookInteractionCommandHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<CreateUserBookInteractionCommand, UserBookInteraction>
{
    public async Task<UserBookInteraction> Handle(CreateUserBookInteractionCommand command, CancellationToken token)
    {
        var interaction = new UserBookInteraction
        {
            InteractionId = Guid.NewGuid(),
            UserId = command.UserId,
            BookId = command.BookId,
            IsFavorite = command.IsFavorite,
            IsSaved = command.IsSaved,
            Status = command.Status,
            Rating = command.Rating
        };
        return await _userBookInteractionRepository.CreateUserBookInteractionAsync(interaction);
    }
} 