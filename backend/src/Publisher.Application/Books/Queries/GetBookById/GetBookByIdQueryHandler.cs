using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetBookByIdQuery, GetBookByIdResponse?>
{
    public async Task<GetBookByIdResponse?> Handle(GetBookByIdQuery query, CancellationToken token)
    {
        var book = await bookRepository.GetBookByIdAsync(query.Id, token)
            ?? throw new NotFoundException(nameof(Book), query.Id);

        // [.. ] is a collection expression
        // Instead of using .ToList()
        return new GetBookByIdResponse(
            book.BookId,
            book.Title,
            book.PublishDate,
            book.BasePrice,
            book.Slug,
            [.. book.Covers.Select(c => new CoverResponse(
                c.CoverId,
                c.ImgBase64 ?? string.Empty,
                c.CreatedDate,
                [.. c.CoverPersons.Select(cp => new ArtistResponse(
                    cp.ArtistPersonId,
                    cp.Artist.FirstName,
                    cp.Artist.LastName,
                    cp.Artist.Email,
                    cp.Artist.Phone ?? string.Empty,
                    cp.Artist.PortfolioUrl ?? string.Empty
                ))]
            ))],
            [.. book.BookPersons.Select(bp => new AuthorResponse(
                bp.AuthorPersonId,
                bp.Author.FirstName,
                bp.Author.LastName,
                bp.Author.Email,
                bp.Author.Phone ?? string.Empty,
                bp.Author.RoyaltyRate
            ))],
            [.. book.BookGenres.Select(bg => new GenreResponse(
                bg.GenreId,
                bg.Genre?.Name ?? string.Empty
            ))]
        );
    }
}
