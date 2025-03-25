using MediatR;
using Publisher.Application.Interfaces;

namespace Publisher.Application.Authentication.Commands.Logout;

public class LogoutCommandHandler(IUserRepository userRepository, ICurrentUserService currentUserService) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken token)
    {
        var userId = currentUserService.GetCurrentUserId();

        var user = await userRepository.GetUserByIdAsync(userId, token)
            ?? throw new UnauthorizedAccessException("User not found.");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        await userRepository.UpdateUserAsync(userId, user, token);
    }
}
