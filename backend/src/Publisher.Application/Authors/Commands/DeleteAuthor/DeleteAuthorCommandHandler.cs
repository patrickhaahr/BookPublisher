using MediatR;
using Publisher.Application.Interfaces;
using Publisher.Domain.Entities;
using Publisher.Domain.Exceptions;

namespace Publisher.Application.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler(IAuthorRepository _authorRepository)
    : IRequestHandler<DeleteAuthorCommand, Author>
{
    public async Task<Author> Handle(DeleteAuthorCommand command, CancellationToken token)
    {
        var deletedAuthor = await _authorRepository.DeleteAuthorAsync(command.Id);

        return deletedAuthor is null
            ? throw new NotFoundException(nameof(Author), command.Id)
            : deletedAuthor;
    }
}