using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Authors.Queries.GetAuthors;

public record GetAuthorsQuery(
    int Page = 1,
    int PageSize = 10
) : IRequest<PaginatedResponse<AuthorResponse>>;

