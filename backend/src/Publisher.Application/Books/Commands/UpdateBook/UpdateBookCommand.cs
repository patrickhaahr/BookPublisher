using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    DateTime PublishDate,
    decimal BasePrice)
    : IRequest<UpdateBookResponse>;
