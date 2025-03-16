using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid Id,
    string Username,
    string Email,
    string PasswordHash,
    string Role) : IRequest<User>; 