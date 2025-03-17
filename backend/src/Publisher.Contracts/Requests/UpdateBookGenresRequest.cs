namespace Publisher.Contracts.Requests;

public record UpdateBookGenresRequest(
    List<int> GenreIds
);


