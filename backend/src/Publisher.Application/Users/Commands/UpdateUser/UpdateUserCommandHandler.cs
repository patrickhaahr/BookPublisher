using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository _userRepository)
    : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    public async Task<UpdateUserResponse> Handle(UpdateUserCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetUserByIdAsync(command.Id, token)
            ?? throw new NotFoundException(nameof(User), command.Id);

        // Validate username uniqueness if it's being updated
        if (command.Username is not null)
        {
            var existingUsername = await _userRepository.GetUserByUsernameAsync(command.Username, token);
            if (existingUsername is not null && existingUsername.UserId != command.Id)
            {
                throw new ValidationException("Username", "Username is already taken.");
            }
        }

        // Validate email uniqueness if it's being updated
        if (command.Email is not null)
        {
            var existingEmail = await _userRepository.GetUserByEmailAsync(command.Email, token);
            if (existingEmail is not null && existingEmail.UserId != command.Id)
            {
                throw new ValidationException("Email", "Email address is already taken.");
            }
        }

        user.Username = command.Username ?? user.Username;
        user.Email = command.Email ?? user.Email;
        user.PasswordHash = command.PasswordHash ?? user.PasswordHash;

        var updatedUser = await _userRepository.UpdateUserAsync(command.Id, user, token);

        return updatedUser is null
            ? throw new NotFoundException(nameof(User), command.Id)
            : new UpdateUserResponse(updatedUser.UserId, updatedUser.Username, updatedUser.Email, updatedUser.Role);
    }
} 