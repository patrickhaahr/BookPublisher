using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<User>; 