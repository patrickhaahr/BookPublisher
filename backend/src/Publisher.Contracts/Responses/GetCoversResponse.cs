namespace Publisher.Contracts.Responses;

public record GetCoversResponse(
    string BookId,
    string ImgBase64
);

