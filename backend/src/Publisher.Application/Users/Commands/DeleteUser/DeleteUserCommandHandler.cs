using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository _userRepository)
    : IRequestHandler<DeleteUserCommand, User>
{
    public async Task<User> Handle(DeleteUserCommand command, CancellationToken token)
    {
        var deletedUser = await _userRepository.DeleteUserAsync(command.Id, token);

        return deletedUser is null
            ? throw new NotFoundException(nameof(User), command.Id)
            : deletedUser;
    }
} 