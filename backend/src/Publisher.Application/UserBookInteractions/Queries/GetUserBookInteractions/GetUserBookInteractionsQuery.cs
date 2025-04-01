using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;

public record GetUserBookInteractionsQuery : IRequest<List<UserBookInteractionResponse>>; 