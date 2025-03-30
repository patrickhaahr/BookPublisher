namespace Publisher.Contracts.Requests;

public record UpdateBookCoversRequest(
    List<CoverUpdateData> Covers
);