using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(string IdOrSlug) : IRequest<DeleteResponse>;

