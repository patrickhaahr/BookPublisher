using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid Id) : IRequest<Book>;

