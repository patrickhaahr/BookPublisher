using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Commands.DeleteCover;

public record DeleteCoverCommand(Guid Id) : IRequest<Cover>; 