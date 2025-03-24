using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Interfaces.Authentication;

namespace Publisher.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RefreshTokenCommand command, CancellationToken token)
    {
        var user = await userRepository.GetUserByIdAsync(command.UserId, token);
        if (user is null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var accessToken = jwtTokenGenerator.GenerateToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        await userRepository.UpdateUserAsync(user.UserId, user, token);

        return new AuthenticationResult(user, accessToken, refreshToken);
    }
}
