using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<UpdateAuthorCommand, Author>
{
    public async Task<Author> Handle(UpdateAuthorCommand command, CancellationToken token)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(command.Id)
            ?? throw new NotFoundException(nameof(Author), command.Id);

        author.FirstName = command.FirstName;
        author.LastName = command.LastName;
        author.Email = command.Email;
        author.Phone = command.Phone;
        author.RoyaltyRate = command.RoyaltyRate;

        var updatedAuthor = await _authorRepository.UpdateAuthorAsync(command.Id, author);

        return updatedAuthor is null
            ? throw new NotFoundException(nameof(Author), command.Id)
            : updatedAuthor;
    }
}