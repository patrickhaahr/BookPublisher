using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string Email,
    string PasswordHash,
    string Role) : IRequest<User>; 