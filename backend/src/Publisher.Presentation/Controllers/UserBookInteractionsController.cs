using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;
using Publisher.Application.UserBookInteractions.Commands.DeleteUserBookInteraction;
using Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;
using Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;
using Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;
using Publisher.Contracts.Responses;
using Publisher.Presentation.Authorization;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class UserBookInteractionsController(ISender _sender) : ControllerBase
{
    [JwtAuthorize]
    [HttpGet(ApiEndpoints.V1.UserBookInteractions.GetAll)]
    public async Task<IActionResult> GetUserBookInteractions(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetUserBookInteractionsQuery(), token));
    }

    [JwtAuthorize]
    [HttpGet(ApiEndpoints.V1.UserBookInteractions.GetById)]
    public async Task<IActionResult> GetUserBookInteractionById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetUserBookInteractionByIdQuery(id), token));
    }

    [JwtAuthorize]
    [HttpPost(ApiEndpoints.V1.UserBookInteractions.Create)]
    public async Task<IActionResult> CreateUserBookInteraction(
        [FromBody] CreateUserBookInteractionCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command, token));
    }

    [JwtAuthorize]
    [HttpPut(ApiEndpoints.V1.UserBookInteractions.Update)]
    public async Task<IActionResult> UpdateUserBookInteraction(
        [FromRoute] Guid id, [FromBody] UpdateUserBookInteractionCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command with { Id = id }, token));
    }

    [JwtAuthorize]
    [HttpDelete(ApiEndpoints.V1.UserBookInteractions.Delete)]
    public async Task<IActionResult> DeleteUserBookInteraction(
        [FromRoute] Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteUserBookInteractionCommand(id), token);
        return Ok();
    }
}
