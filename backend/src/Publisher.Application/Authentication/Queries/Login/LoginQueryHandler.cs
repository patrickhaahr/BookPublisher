using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Interfaces.Authentication;

namespace Publisher.Application.Authentication.Queries.Login;

public class LoginQueryHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginQuery, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken token)
    {
        var user = await userRepository.GetUserByEmailAsync(query.Email, token);

        if (user is null || !passwordHasher.VerifyPassword(query.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        var accessToken = jwtTokenGenerator.GenerateToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await userRepository.UpdateUserAsync(user.UserId, user, token);

        return new AuthenticationResult(user, accessToken, refreshToken);
    }
}
