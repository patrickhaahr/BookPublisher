namespace Publisher.Contracts.Responses;

public record GetBooksResponse(
    Guid BookId,
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    string Slug,
    List<string> Mediums,
    List<string> Genres,
    List<CoversResponse> Covers,
    List<BookAuthorResponse> Authors
);

public record CoversResponse(
    Guid CoverId,
    string ImgBase64
);

public record BookAuthorResponse(
    Guid AuthorId,
    string FirstName,
    string LastName
);

