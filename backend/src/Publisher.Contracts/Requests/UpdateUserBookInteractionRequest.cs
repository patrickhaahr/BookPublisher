namespace Publisher.Contracts.Requests;

public record UpdateUserBookInteractionRequest(
    bool? IsFavorite = null,
    bool? IsSaved = null,
    string? Status = null,
    int? Rating = null
); 