using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Interfaces.Authentication;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken token)
    {

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            Role = request.Role
        };

        await userRepository.CreateUserAsync(user, token);

        var accessToken = jwtTokenGenerator.GenerateToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await userRepository.UpdateUserAsync(user.UserId, user, token);

        return new AuthenticationResult(user, accessToken, refreshToken);
    }
}
