using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.UserBookInteractions.Commands.CreateUserBookInteraction;
using Publisher.Application.UserBookInteractions.Commands.DeleteUserBookInteraction;
using Publisher.Application.UserBookInteractions.Commands.UpdateUserBookInteraction;
using Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;
using Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;
using Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionByUserAndBook;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;
using Publisher.Presentation.Authorization;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class UserBookInteractionsController(ISender _sender) : ControllerBase
{
    [JwtAuthorize]
    [HttpGet(ApiEndpoints.V1.UserBookInteractions.GetAll)]
    public async Task<IActionResult> GetUserBookInteractions([FromQuery] string? bookId, CancellationToken token)
    {
        var interactions = await _sender.Send(new GetUserBookInteractionsQuery(), token);
        
        // Filter by bookId if provided
        if (!string.IsNullOrEmpty(bookId))
        {
            // Try to parse as Guid first
            if (Guid.TryParse(bookId, out var bookGuid))
            {
                interactions = interactions.Where(i => i.BookId == bookGuid).ToList();
            }
            else
            {
                // Assume it's a slug and filter by book.slug
                interactions = interactions.Where(i => i.Book?.Slug == bookId).ToList();
            }
        }

        return Ok(interactions);
    }

    [JwtAuthorize]
    [HttpGet(ApiEndpoints.V1.UserBookInteractions.GetUserAndBook)]
    public async Task<IActionResult> GetUserBookInteractionByUserAndBook(
        [FromQuery] Guid userId, [FromQuery] string bookId, CancellationToken token)
    {
        var interaction = await _sender.Send(new GetUserBookInteractionByUserAndBookQuery(userId, bookId), token);
        
        if (interaction is null)
            return NotFound();
            
        return Ok(interaction);
    }

    [JwtAuthorize]
    [HttpGet(ApiEndpoints.V1.UserBookInteractions.GetById)]
    public async Task<IActionResult> GetUserBookInteractionById(
        [FromRoute] Guid id, CancellationToken token)
    {
        var interaction = await _sender.Send(new GetUserBookInteractionByIdQuery(id), token);
        return Ok(interaction);
    }

    [JwtAuthorize]
    [HttpPost(ApiEndpoints.V1.UserBookInteractions.Create)]
    public async Task<IActionResult> CreateUserBookInteraction(
        [FromBody] CreateUserBookInteractionRequest request, CancellationToken token)
    {
        var command = new CreateUserBookInteractionCommand(
            request.UserId,
            request.BookId,
            request.IsFavorite,
            request.IsSaved,
            request.Status,
            request.Rating
        );
        
        var interaction = await _sender.Send(command, token);
        return Ok(interaction);
    }

    [JwtAuthorize]
    [HttpPut(ApiEndpoints.V1.UserBookInteractions.Update)]
    public async Task<IActionResult> UpdateUserBookInteraction(
        [FromRoute] Guid id, [FromBody] UpdateUserBookInteractionRequest request, CancellationToken token)
    {
        // Get the existing interaction first
        var existingInteraction = await _sender.Send(new GetUserBookInteractionByIdQuery(id), token);

        // Apply partial updates, keeping existing values if not provided
        var command = new UpdateUserBookInteractionCommand(
            id,
            existingInteraction.UserId,
            existingInteraction.BookId,
            request.IsFavorite,
            request.IsSaved,
            request.Status,
            request.Rating
        );
        
        var updatedInteraction = await _sender.Send(command, token);
        return Ok(updatedInteraction);
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
