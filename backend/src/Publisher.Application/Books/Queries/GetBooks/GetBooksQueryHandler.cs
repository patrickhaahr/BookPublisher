using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetBooksQuery, PagedResponse<GetBooksResponse>>
{
    public async Task<PagedResponse<GetBooksResponse>> Handle(GetBooksQuery query, CancellationToken token)
    {
        // Fetch paginated books and total count
        var (books, totalCount) = await bookRepository.GetBooksPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Title,
            query.Author,
            query.Genre,
            query.Medium,
            query.Year,
            token);

        // Map books to responses
        var bookResponses = books.Select(book => new GetBooksResponse(
            book.BookId,
            book.Title,
            book.PublishDate,
            book.BasePrice,
            book.Slug,
            [.. book.BookMediums.Select(m => m.Medium.Name)],
            [.. book.BookGenres.Select(g => g.Genre.Name)],
            [.. book.Covers.Select(c => new CoversResponse(c.CoverId, c.ImgBase64 ?? string.Empty))],
            [.. book.BookPersons.Select(bp => new BookAuthorResponse(bp.Author.PersonId, bp.Author.FirstName, bp.Author.LastName))]
        )).ToList();
        
        // Calculate pagination metadata
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        // Return the paged response
        return new PagedResponse<GetBooksResponse>(
            Items: bookResponses,
            Total: totalCount,
            Page: query.Page,
            PageSize: query.PageSize,
            TotalPages: totalPages,
            HasNextPage: query.Page < totalPages,
            HasPreviousPage: query.Page > 1
        );
    }
}
