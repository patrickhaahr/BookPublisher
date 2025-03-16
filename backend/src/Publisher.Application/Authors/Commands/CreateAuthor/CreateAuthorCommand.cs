using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Commands.CreateAuthor;

public record CreateAuthorCommand(
    Guid PersonId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    decimal RoyaltyRate) : IRequest<Author>;
