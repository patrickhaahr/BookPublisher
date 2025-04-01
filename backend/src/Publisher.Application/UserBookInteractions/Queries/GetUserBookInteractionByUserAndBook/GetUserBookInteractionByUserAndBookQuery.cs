using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionByUserAndBook;

public record GetUserBookInteractionByUserAndBookQuery(
    Guid UserId,
    string BookId) : IRequest<UserBookInteractionResponse?>; 