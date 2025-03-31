using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Artists.Commands.CreateArtist;
using Publisher.Application.Artists.Commands.DeleteArtist;
using Publisher.Application.Artists.Commands.UpdateArtist;
using Publisher.Application.Artists.Queries.GetArtistById;
using Publisher.Application.Artists.Queries.GetArtists;
using Publisher.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Publisher.Contracts.Requests;
namespace Publisher.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
public class ArtistsController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Artists.GetAll)]
    public async Task<IActionResult> GetArtists(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default)
    {
        return Ok(await _sender.Send(new GetArtistsQuery(page, pageSize), token));
    }

    [HttpGet(ApiEndpoints.V1.Artists.GetById)]
    public async Task<IActionResult> GetArtistById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetArtistByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Artists.Create)]
    public async Task<IActionResult> CreateArtist(
        [FromBody] CreateArtistRequest request, CancellationToken token)
    {
        var command = new CreateArtistCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone ?? string.Empty,
            request.PortfolioUrl ?? string.Empty
        );
        return Ok(await _sender.Send(command, token));
    }

    [HttpPut(ApiEndpoints.V1.Artists.Update)]
    public async Task<IActionResult> UpdateArtist(
        [FromRoute] Guid id, [FromBody] UpdateArtistCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Artists.Delete)]
    public async Task<IActionResult> DeleteArtist(
        [FromRoute] Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteArtistCommand(id), token);
        return Ok(new DeleteResponse());
    }
}