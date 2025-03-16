using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetBooksQuery, List<Book>>
{
    public async Task<List<Book>> Handle(GetBooksQuery query, CancellationToken token)
    {
        return await bookRepository.GetBooksAsync();
    }
}
