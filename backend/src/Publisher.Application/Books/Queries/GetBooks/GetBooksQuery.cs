using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBooks;

public record GetBooksQuery(int Page = 1, int PageSize = 12) : IRequest<PagedResponse<GetBooksResponse>>;

