using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;

namespace Publisher.Application.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<CreateAuthorCommand, Author>
{
    public async Task<Author> Handle(CreateAuthorCommand command, CancellationToken token)
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
        return await _authorRepository.CreateAuthorAsync(author);
    }
}
