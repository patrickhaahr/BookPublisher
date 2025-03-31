using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Contracts.Responses;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<CreateAuthorCommand, AuthorResponse>
{
    public async Task<AuthorResponse> Handle(CreateAuthorCommand command, CancellationToken token)
    {
        var author = new Author
        {
            PersonId = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Phone = command.Phone,
            RoyaltyRate = command.RoyaltyRate
        };
        await _authorRepository.CreateAuthorAsync(author, token);

        return new AuthorResponse(
            author.PersonId,
            author.FirstName,
            author.LastName,
            author.Email,
            author.Phone ?? string.Empty,
            author.RoyaltyRate
        );
    }
}
