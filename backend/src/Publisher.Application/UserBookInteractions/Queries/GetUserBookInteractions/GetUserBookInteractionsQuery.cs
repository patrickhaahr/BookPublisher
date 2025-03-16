using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractions;

public record GetUserBookInteractionsQuery : IRequest<List<UserBookInteraction>>; 