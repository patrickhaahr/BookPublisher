using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookAuthors;

public record UpdateBookAuthorsCommand(
    Guid BookId,
    List<Guid> AuthorIds
) : IRequest<UpdateBookAuthorsResponse>; 