using MediatR;
using Microsoft.AspNetCore.Mvc;
using Publisher.Application.Books.Commands.CreateBook;
using Publisher.Application.Books.Commands.DeleteBook;
using Publisher.Application.Books.Commands.UpdateBook;
using Publisher.Application.Books.Queries.GetBooks;
using Publisher.Application.Books.Queries.GetBookById;
using Publisher.Contracts.Requests;
using Publisher.Contracts.Responses;
using Publisher.Presentation.Authorization;

namespace Publisher.Presentation.Controllers;

[ApiController]
public class BooksController(ISender _sender) : ControllerBase
{
    [HttpGet(ApiEndpoints.V1.Books.GetAll)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? title = null,   
        [FromQuery] string? author = null,
        [FromQuery] string? genre = null,
        [FromQuery] string? medium = null,
        [FromQuery] string? year = null,
        CancellationToken token = default)
    {
        return Ok(await _sender.Send(new GetBooksQuery(page, pageSize, title, author, genre, medium, year), token));
    }

    [HttpGet(ApiEndpoints.V1.Books.GetById)]
    public async Task<IActionResult> GetBookById(
        [FromRoute] string id, CancellationToken token)
    {
        return Ok(await _sender.Send(new GetBookByIdQuery(id), token));
    }

    [JwtAdmin]
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
            request.Mediums,
            request.Genres,
            request.AuthorIds,
            covers
        );
        var result = await _sender.Send(command, token);
        return CreatedAtAction(nameof(GetBookById), new { id = result.BookId }, result);
    }

    [JwtAdmin]
    [HttpPut(ApiEndpoints.V1.Books.Update)]
    public async Task<IActionResult> UpdateBook(
        [FromRoute] string id, [FromBody] UpdateBookRequest request, CancellationToken token)
    {
        var command = new UpdateBookCommand(
            id,
            request.Title,
            request.PublishDate,
            request.BasePrice,
            request.Mediums,
            request.Genres,
            request.AuthorIds,
            request.Covers
        );
        return Ok(await _sender.Send(command, token));
    }

    [JwtAdmin]
    [HttpDelete(ApiEndpoints.V1.Books.Delete)]
    public async Task<IActionResult> DeleteBook(
        [FromRoute] string id, CancellationToken token)
    {
        await _sender.Send(new DeleteBookCommand(id), token);
        return Ok(new DeleteResponse());
    }
}

