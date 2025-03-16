using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Authors.Queries.GetAuthorById;

public class GetAuthorByIdQueryHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<GetAuthorByIdQuery, Author>
{
    public async Task<Author> Handle(GetAuthorByIdQuery query, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(query.Id);
        
        return author is null
            ? throw new NotFoundException(nameof(Author), query.Id)
            : author;
    }
}
