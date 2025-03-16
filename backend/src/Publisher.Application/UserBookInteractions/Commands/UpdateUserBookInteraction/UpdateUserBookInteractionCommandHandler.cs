using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;

public class UpdateUserBookInteractionCommandHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<UpdateUserBookInteractionCommand, UserBookInteraction>
{
    public async Task<UserBookInteraction> Handle(UpdateUserBookInteractionCommand command, CancellationToken token)
    {
        var interaction = await _userBookInteractionRepository.GetUserBookInteractionByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(UserBookInteraction), command.Id);

        interaction.UserId = command.UserId;
        interaction.BookId = command.BookId;
        interaction.IsFavorite = command.IsFavorite;
        interaction.IsSaved = command.IsSaved;
        interaction.Status = command.Status;
        interaction.Rating = command.Rating;

        var updatedInteraction = await _userBookInteractionRepository.UpdateUserBookInteractionAsync(command.Id, interaction);

        return updatedInteraction is null
            ? throw new NotFoundException(nameof(UserBookInteraction), command.Id)
            : updatedInteraction;
    }
} 