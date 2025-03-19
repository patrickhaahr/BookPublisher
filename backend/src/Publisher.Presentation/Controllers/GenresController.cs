using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Genres.Commands.CreateGenre;
using Publisher.Application.Genres.Commands.DeleteGenre;
using Publisher.Application.Genres.Commands.UpdateGenre;
using Publisher.Application.Genres.Queries.GetGenreById;
using Publisher.Application.Genres.Queries.GetGenres;
using Publisher.Contracts.Responses;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class GenresController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Genres.GetAll)]
    public async Task<IActionResult> GetGenres(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetGenresQuery(), token));
    }

    [HttpGet(ApiEndpoints.V1.Genres.GetById)]
    public async Task<IActionResult> GetGenreById(
        [FromRoute] int id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetGenreByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Genres.Create)]
    public async Task<IActionResult> CreateGenre(
        [FromBody] CreateGenreCommand command, CancellationToken token)
    {
        return Ok(await _sender.Send(command, token));
    }

    [HttpPut(ApiEndpoints.V1.Genres.Update)]
    public async Task<IActionResult> UpdateGenre(
        [FromRoute] int id, [FromBody] UpdateGenreCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Genres.Delete)]
    public async Task<IActionResult> DeleteGenre(
        [FromRoute] int id, CancellationToken token)
    {
        await _sender.Send(new DeleteGenreCommand(id), token);
        return Ok(new DeleteResponse());
    }
}