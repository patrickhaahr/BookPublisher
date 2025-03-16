using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Covers.Queries.GetCoverById;

public record GetCoverByIdQuery(Guid Id) : IRequest<Cover>; 