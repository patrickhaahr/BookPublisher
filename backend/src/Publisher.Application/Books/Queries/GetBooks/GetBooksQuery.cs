using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBooks;

public record GetBooksQuery(
    int Page = 1,
    int PageSize = 12,
    string? Title = null,
    string? Author = null,
    string? Genre = null,
    string? Medium = null,
    string? Year = null
) : IRequest<PaginatedResponse<GetBooksResponse>>;

