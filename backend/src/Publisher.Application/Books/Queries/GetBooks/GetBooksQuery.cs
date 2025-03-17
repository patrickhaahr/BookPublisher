using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Queries.GetBooks;

public record GetBooksQuery : IRequest<GetBooksResponse>;

