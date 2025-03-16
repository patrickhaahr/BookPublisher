using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<GetAuthorsQuery, List<Author>>
{
    public async Task<List<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return await _authorRepository.GetAuthorsAsync();
    }
}
