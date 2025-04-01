namespace Publisher.Contracts.Requests;

public record CreateUserBookInteractionRequest(
    Guid UserId,
    object BookId,
    bool? IsFavorite = false,
    bool? IsSaved = false,
    string? Status = null,
    int? Rating = null
); 