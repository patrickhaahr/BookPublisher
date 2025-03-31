using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string? Username,
    string? Email,
    string? PasswordHash) : IRequest<UpdateUserResponse>; 