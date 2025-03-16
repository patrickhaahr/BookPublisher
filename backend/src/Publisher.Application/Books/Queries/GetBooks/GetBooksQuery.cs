using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Books.Queries.GetBooks;

public record GetBooksQuery : IRequest<List<Book>>;

