using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<DeleteBookResponse>;

