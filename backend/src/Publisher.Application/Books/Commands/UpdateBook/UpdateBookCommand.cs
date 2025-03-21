using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    string IdOrSlug,
    string? Title,
    DateTime? PublishDate,
    decimal? BasePrice,
    List<string>? Mediums,
    List<string>? Genres
) : IRequest<UpdateBookResponse>;
        