using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Users.Commands.UpdateUserRole;

public sealed class UpdateUserRoleCommandHandler(IUserRepository _userRepository)
    : IRequestHandler<UpdateUserRoleCommand, GetUsersResponse>
{
    public async Task<GetUsersResponse> Handle(UpdateUserRoleCommand command, CancellationToken token)
    {
        var user = await _userRepository.GetUserByIdAsync(command.UserId, token)
            ?? throw new NotFoundException(nameof(User), command.UserId);

        // Only update the role field
        user.Role = command.Role;

        var updatedUser = await _userRepository.UpdateUserAsync(command.UserId, user, token);

        return updatedUser is null
            ? throw new NotFoundException(nameof(User), command.UserId)
            : new GetUsersResponse(
                updatedUser.UserId,
                updatedUser.Username,
                updatedUser.Email,
                updatedUser.Role);
    }
} 