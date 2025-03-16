using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Covers.Commands.CreateCover;
using Publisher.Application.Covers.Commands.DeleteCover;
using Publisher.Application.Covers.Commands.UpdateCover;
using Publisher.Application.Covers.Queries.GetCoverById;
using Publisher.Application.Covers.Queries.GetCovers;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class CoversController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Covers.GetAll)]
    public async Task<IActionResult> GetCovers(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetCoversQuery(), token));
    }

    [HttpGet(ApiEndpoints.V1.Covers.GetById)]
    public async Task<IActionResult> GetCoverById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetCoverByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Covers.Create)]
    public async Task<IActionResult> CreateCover(
        [FromBody] CreateCoverCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command, token));
    }

    [HttpPut(ApiEndpoints.V1.Covers.Update)]
    public async Task<IActionResult> UpdateCover(
        [FromRoute] Guid id, [FromBody] UpdateCoverCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Covers.Delete)]
    public async Task<IActionResult> DeleteCover(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new DeleteCoverCommand(id), token));
    }
}
