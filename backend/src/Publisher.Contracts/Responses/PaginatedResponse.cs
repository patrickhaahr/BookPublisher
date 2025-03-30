namespace Publisher.Contracts.Responses;

public record PaginatedResponse<T>(
    List<T> Items,
    int Total,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage); 