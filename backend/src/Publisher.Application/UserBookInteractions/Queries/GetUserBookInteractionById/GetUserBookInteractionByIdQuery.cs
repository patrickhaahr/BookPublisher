using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Queries.GetUserBookInteractionById;

public record GetUserBookInteractionByIdQuery(Guid Id) : IRequest<UserBookInteraction>; 