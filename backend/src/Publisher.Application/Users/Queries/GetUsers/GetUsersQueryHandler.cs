using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler(IUserRepository _userRepository)
    : IRequestHandler<GetUsersQuery, PaginatedResponse<GetUsersResponse>>
{
    public async Task<PaginatedResponse<GetUsersResponse>> Handle(GetUsersQuery request, CancellationToken token)
    {
        var (users, totalCount, totalPages) = await _userRepository.GetUsersAsync(
            request.Page, 
            request.PageSize, 
            token);
            
        var items = users.Select(user => new GetUsersResponse(
            user.UserId,
            user.Username,
            user.Email,
            user.Role
        )).ToList();
        
        bool hasNextPage = request.Page < totalPages;
        bool hasPrevPage = request.Page > 1;
        
        return new PaginatedResponse<GetUsersResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages,
            hasNextPage,
            hasPrevPage);
    }
} 