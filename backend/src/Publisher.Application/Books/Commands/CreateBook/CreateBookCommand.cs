using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    List<int> GenreIds,
    List<Guid> AuthorIds
) : IRequest<CreateBookResponse>;