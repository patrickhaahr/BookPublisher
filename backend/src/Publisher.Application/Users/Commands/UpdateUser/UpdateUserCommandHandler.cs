using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository _userRepository)
    : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetUserByIdAsync(command.Id, token)
            ?? throw new NotFoundException(nameof(User), command.Id);

        user.Username = command.Username;
        user.Email = command.Email;
        user.PasswordHash = command.PasswordHash;
        user.Role = command.Role;

        var updatedUser = await _userRepository.UpdateUserAsync(command.Id, user, token);

        return updatedUser is null
            ? throw new NotFoundException(nameof(User), command.Id)
            : updatedUser;
    }
} 