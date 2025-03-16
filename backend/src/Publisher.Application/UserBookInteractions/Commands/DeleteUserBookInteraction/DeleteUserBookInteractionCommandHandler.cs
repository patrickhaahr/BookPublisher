using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.UserBookInteractions.Commands.DeleteUserBookInteraction;

public class DeleteUserBookInteractionCommandHandler(IUserBookInteractionRepository _userBookInteractionRepository)
    : IRequestHandler<DeleteUserBookInteractionCommand, UserBookInteraction>
{
    public async Task<UserBookInteraction> Handle(DeleteUserBookInteractionCommand command, CancellationToken token)
    {
        var deletedInteraction = await _userBookInteractionRepository.DeleteUserBookInteractionAsync(command.Id);

        return deletedInteraction is null
            ? throw new NotFoundException(nameof(UserBookInteraction), command.Id)
            : deletedInteraction;
    }
} 