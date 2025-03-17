using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Books.Commands.CreateBook;
using Publisher.Application.Books.Commands.DeleteBook;
using Publisher.Application.Books.Commands.UpdateBook;
using Publisher.Application.Books.Queries.GetBookById;
using Publisher.Application.Books.Queries.GetBooks;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;
using System.Text.Json;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class BooksController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Books.GetAll)]
    public async Task<IActionResult> GetBooks(CancellationToken token)
    {
        return Ok(await _sender.Send(new GetBooksQuery(), token));
    }

    [HttpGet(ApiEndpoints.V1.Books.GetById)]
    public async Task<IActionResult> GetBookById(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetBookByIdQuery(id), token));
    }

    [HttpPost(ApiEndpoints.V1.Books.Create)]
    public async Task<IActionResult> CreateBook(
        [FromBody] CreateBookRequest request, CancellationToken token)
    {
        var covers = request.Covers?.Select(c => new CoverData(c.ImgBase64, c.ArtistIds)).ToList() 
            ?? []; // new List<CoverData>();

        //var covers = new List<CoverData>();
            
        var command = new CreateBookCommand(
            request.Title,
            request.PublishDate,
            request.BasePrice,
            request.GenreIds,
            request.AuthorIds,
            covers
        );
        var result = await _sender.Send(command, token);
        return CreatedAtAction(nameof(GetBookById), new { id = result.BookId }, result);
    }

    [HttpPut(ApiEndpoints.V1.Books.Update)]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] Guid id, [FromBody] UpdateBookCommand command, CancellationToken token)
    {
        var updateCommand = command with { Id = id };
        return Ok(await _sender.Send(updateCommand, token));
    }

    [HttpDelete(ApiEndpoints.V1.Books.Delete)]
    public async Task<IActionResult> DeleteBook(
        [FromRoute] Guid id, CancellationToken token)
    {
        return Ok(await _sender.Send(new DeleteBookCommand(id), token));
    }
}

