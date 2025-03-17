namespace Publisher.Contracts.Responses;

public record UpdateBookGenresResponse(
    Guid BookId,
    List<int> GenreIds
);

