using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Queries.GetAuthorById;

public record GetAuthorByIdQuery(Guid Id) : IRequest<Author>;

