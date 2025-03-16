using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository _userRepository)
    : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand command, CancellationToken token)
    {
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = command.Username,
            Email = command.Email,
            PasswordHash = command.PasswordHash,
            Role = command.Role
        };
        return await _userRepository.CreateUserAsync(user);
    }
} 