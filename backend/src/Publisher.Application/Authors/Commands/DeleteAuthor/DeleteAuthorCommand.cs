using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Commands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest<Author>;
