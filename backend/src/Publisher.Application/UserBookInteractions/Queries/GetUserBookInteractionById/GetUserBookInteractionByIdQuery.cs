using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;

public record GetUserBookInteractionByIdQuery(Guid Id) : IRequest<UserBookInteractionResponse>; 