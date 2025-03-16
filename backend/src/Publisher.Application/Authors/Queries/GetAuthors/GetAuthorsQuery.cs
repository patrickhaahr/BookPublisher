using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Queries.GetAuthors;

public record GetAuthorsQuery : IRequest<List<Author>>;

