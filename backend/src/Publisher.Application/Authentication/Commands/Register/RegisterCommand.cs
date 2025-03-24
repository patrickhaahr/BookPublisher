using MediatR;

namespace Publisher.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string Role = "User") : IRequest<AuthenticationResult>;

