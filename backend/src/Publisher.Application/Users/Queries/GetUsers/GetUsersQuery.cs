using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Users.Queries.GetUsers;

public record GetUsersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchQuery = null) : IRequest<PaginatedResponse<GetUsersResponse>>; 