namespace Publisher.Contracts.Requests;

public record UpdateBookRequest(
    string Title,
    DateTime PublishDate,
    decimal BasePrice
); 