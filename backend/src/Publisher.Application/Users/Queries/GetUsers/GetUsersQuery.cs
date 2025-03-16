using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<List<User>>; 