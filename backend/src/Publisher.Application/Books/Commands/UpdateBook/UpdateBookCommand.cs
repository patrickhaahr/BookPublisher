using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    DateTime PublishDate,
    decimal BasePrice)
    : IRequest<Book>;
