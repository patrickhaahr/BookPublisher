using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Users.Commands.CreateUser;
using Publisher.Application.Users.Commands.DeleteUser;
using Publisher.Application.Users.Commands.UpdateUser;
using Publisher.Application.Users.Commands.UpdateUserRole;
using Publisher.Application.Users.Queries.GetUserById;
using Publisher.Application.Users.Queries.GetUsers;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class UsersController(ISender _sender) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet(ApiEndpoints.V1.Users.GetAll)]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string? search = null,
        CancellationToken token = default)
    {
        return Ok(await _sender.Send(new GetUsersQuery(page, pageSize, search), token));
    }

    [Authorize]
    [HttpGet(ApiEndpoints.V1.Users.GetById)]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetUserByIdQuery(id), token));
    }

    [Authorize]
    [HttpPut(ApiEndpoints.V1.Users.Update)]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid id, [FromBody] UpdateUserRequest request, CancellationToken token)
    {
        var command = new UpdateUserCommand(id, request.Username, request.Email, request.PasswordHash);
        return Ok(await _sender.Send(command, token));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut(ApiEndpoints.V1.Users.UpdateRole)]
    public async Task<IActionResult> UpdateUserRole(
        [FromRoute] Guid id, [FromBody] UpdateUserRoleRequest request, CancellationToken token)
    {
        var command = new UpdateUserRoleCommand(id, request.Role);
        return Ok(await _sender.Send(command, token));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete(ApiEndpoints.V1.Users.Delete)]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteUserCommand(id), token);
        return Ok(new DeleteResponse());
    }
}
