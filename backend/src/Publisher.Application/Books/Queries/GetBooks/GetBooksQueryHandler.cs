using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetBooksQuery, GetBooksResponse>
{
    public async Task<GetBooksResponse> Handle(GetBooksQuery query, CancellationToken token)
    {
        var books = await bookRepository.GetBooksAsync();
        
        var bookResponses = books.Select(book => new BookSummaryResponse(
            book.BookId,
            book.Title,
            book.PublishDate,
            book.BasePrice,
            book.Slug,
            [.. book.Mediums.Select(m => m.ToString())],
            [.. book.Genres.Select(g => g.ToString())]
        )).ToList();
        
        return new GetBooksResponse(bookResponses);
    }
}
