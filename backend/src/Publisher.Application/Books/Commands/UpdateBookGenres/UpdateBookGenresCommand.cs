using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookGenres;

public record UpdateBookGenresCommand(
    Guid BookId,
    List<int> GenreIds
) : IRequest<UpdateBookGenresResponse>;
