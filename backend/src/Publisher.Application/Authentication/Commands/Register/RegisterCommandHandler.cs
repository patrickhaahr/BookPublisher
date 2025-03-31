using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Application.Interfaces.Authentication;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken token)
    {
        // Check if username is already taken
        var existingUsername = await userRepository.GetUserByUsernameAsync(request.Username, token);
        if (existingUsername is not null)
        {
            throw new ValidationException("Username", "Username is already taken.");
        }

        // Check if email is already taken
        var existingEmail = await userRepository.GetUserByEmailAsync(request.Email, token);
        if (existingEmail is not null)
        {
            throw new ValidationException("Email", "Email address is already taken.");
        }

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
