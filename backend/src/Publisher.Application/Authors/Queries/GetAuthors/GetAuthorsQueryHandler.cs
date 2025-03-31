using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Authors.Queries.GetAuthors;

public class GetAuthorsQueryHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<GetAuthorsQuery, PaginatedResponse<AuthorResponse>>
{
    public async Task<PaginatedResponse<AuthorResponse>> Handle(GetAuthorsQuery request, CancellationToken token)
    {
        var (authors, totalCount, totalPages) = await _authorRepository.GetAuthorsAsync(
            request.Page,
            request.PageSize,
            token);

        var items = authors.Select(author => new AuthorResponse(
            author.PersonId,
            author.FirstName,
            author.LastName,
            author.Email,
            author.Phone ?? string.Empty,   
            author.RoyaltyRate
        )).ToList();

        bool hasNextPage = request.Page < totalPages;
        bool hasPrevPage = request.Page > 1;

        return new PaginatedResponse<AuthorResponse>(
            items,
            request.Page,
            request.PageSize,
            totalCount,
            totalPages,
            hasNextPage,
            hasPrevPage
        );
    }
}
