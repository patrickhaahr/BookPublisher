using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBookGenres;

public record UpdateBookGenresCommand(
    string IdOrSlug,
    List<int> GenreIds
) : IRequest<UpdateBookGenresResponse>;
