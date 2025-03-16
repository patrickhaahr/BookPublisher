using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.UserBookInteractions.Commands.DeleteUserBookInteraction;

public record DeleteUserBookInteractionCommand(Guid Id) : IRequest<UserBookInteraction>; 