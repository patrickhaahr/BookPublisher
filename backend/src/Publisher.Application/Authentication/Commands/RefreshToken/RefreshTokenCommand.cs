using MediatR;

namespace Publisher.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(
    Guid UserId,
    string RefreshToken
) : IRequest<AuthenticationResult>;



