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
        // Try to parse as GUID first, if successful use GetBookByIdAsync, otherwise use GetBookBySlugAsync
        Book? book = Guid.TryParse(query.IdOrSlug, out var id)
            ? await bookRepository.GetBookByIdAsync(id, token)
            : await bookRepository.GetBookBySlugAsync(query.IdOrSlug, token);

        if (book is null)
            throw new NotFoundException(nameof(Book), query.IdOrSlug);

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
            [.. book.BookMediums.Select(m => m.Medium.Name)],
            [.. book.BookGenres.Select(g => g.Genre.Name)]
        );
    }
}
