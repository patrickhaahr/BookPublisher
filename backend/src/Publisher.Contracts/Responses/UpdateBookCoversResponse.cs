using Publisher.Contracts.Requests;

namespace Publisher.Contracts.Responses;

public record UpdateBookCoversResponse(
    Guid BookId,
    List<CoverUpdateData> Covers
);