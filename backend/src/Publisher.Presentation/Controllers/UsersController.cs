using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Users.Commands.CreateUser;
using Publisher.Application.Users.Commands.DeleteUser;
using Publisher.Application.Users.Commands.UpdateUser;
using Publisher.Application.Users.Queries.GetUserById;
using Publisher.Application.Users.Queries.GetUsers;
using Publisher.Contracts.Responses;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class UsersController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Users.GetAll)]
    public async Task<IActionResult> GetUsers(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetUsersQuery(), token));
    }

    [HttpGet(ApiEndpoints.V1.Users.GetById)]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetUserByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Users.Create)]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command, token));
    }

    [HttpPut(ApiEndpoints.V1.Users.Update)]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid id, [FromBody] UpdateUserCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Users.Delete)]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteUserCommand(id), token);
        return Ok(new DeleteResponse());
    }
}
