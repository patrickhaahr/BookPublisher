namespace Publisher.Contracts.Responses;

public record GetBookByIdResponse(
    Guid BookId,
    string Title,
    DateTime PublishDate,
    decimal BasePrice,
    string Slug,
    List<CoverResponse> Covers,
    List<AuthorResponse> Authors,
    List<string> Mediums,
    List<string> Genres
);