namespace Publisher.Contracts.Responses;

public record UserBookInteractionResponse(
    Guid InteractionId,
    Guid UserId,
    Guid BookId,
    bool IsFavorite,
    bool IsSaved,
    string? Status,
    int? Rating,
    UserBriefResponse? User,
    BookBriefResponse? Book
);

public record UserBriefResponse(
    Guid UserId,
    string Username,
    string Email
);

public record BookBriefResponse(
    Guid BookId,
    string Title,
    string Slug
); 