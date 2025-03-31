using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Authors.Commands.CreateAuthor;
using Publisher.Application.Authors.Commands.DeleteAuthor;
using Publisher.Application.Authors.Commands.UpdateAuthor;
using Publisher.Application.Authors.Queries.GetAuthorById;
using Publisher.Application.Authors.Queries.GetAuthors;
using Publisher.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
namespace Publisher.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
public class AuthorsController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Authors.GetAll)]
    public async Task<IActionResult> GetAuthors(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default)
    {
        return Ok(await _sender.Send(new GetAuthorsQuery(page, pageSize), token));
    }

    [HttpGet(ApiEndpoints.V1.Authors.GetById)]
    public async Task<IActionResult> GetAuthorById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetAuthorByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Authors.Create)]
    public async Task<IActionResult> CreateAuthor(
        [FromBody] CreateAuthorCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command, token));
    }

    [HttpPut(ApiEndpoints.V1.Authors.Update)]
    public async Task<IActionResult> UpdateAuthor(
        [FromRoute] Guid id, [FromBody] UpdateAuthorCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Authors.Delete)]
    public async Task<IActionResult> DeleteAuthor(
        [FromRoute] Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteAuthorCommand(id), token);
        return Ok(new DeleteResponse());
    }
}
