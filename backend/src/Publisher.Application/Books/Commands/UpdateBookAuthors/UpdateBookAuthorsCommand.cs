using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookAuthors;

public record UpdateBookAuthorsCommand(
    string IdOrSlug,
    List<Guid> AuthorIds
) : IRequest<UpdateBookAuthorsResponse>; 