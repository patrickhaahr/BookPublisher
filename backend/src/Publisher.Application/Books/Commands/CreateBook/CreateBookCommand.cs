using MediatR;
using Publisher.Contracts.Responses;

namespace Publisher.Application.Books.Commands.CreateBook;

public record CoverData(string ImgBase64, List<Guid> ArtistIds);

public record CreateBookCommand(
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    List<int> GenreIds,
    List<Guid> AuthorIds,
    List<CoverData> Covers
) : IRequest<CreateBookResponse>;