using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Authors.Commands.CreateAuthor;

public record CreateAuthorCommand(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    decimal RoyaltyRate) : IRequest<AuthorResponse>;
