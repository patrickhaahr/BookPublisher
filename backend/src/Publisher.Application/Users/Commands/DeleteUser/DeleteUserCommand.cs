using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<User>; 