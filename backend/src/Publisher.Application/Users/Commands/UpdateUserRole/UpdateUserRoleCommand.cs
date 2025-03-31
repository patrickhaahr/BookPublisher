using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Users.Commands.UpdateUserRole;

public record UpdateUserRoleCommand(
    Guid UserId,
    string Role) : IRequest<GetUsersResponse>; 