using MediatR;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Commands.UpdateAuthor;

public record UpdateAuthorCommand(
    Guid Id,
    string FirstName,
    string LastName, 
    string Email, 
    string Phone, 
    decimal RoyaltyRate) : IRequest<Author>;
