using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Covers.Commands.CreateCover;

public class CreateCoverCommandValidator : AbstractValidator<CreateCoverCommand>
{
    public CreateCoverCommandValidator()
    {
        RuleFor(c => c.ImgBase64)
            .NotEmpty()
            .WithMessage("Image base64 is required")
            .Length(64)
            .WithMessage("Image base64 must be 64 characters long")
            .Must(Validation.IsValidBase64Image)
            .WithMessage("Image base64 must be a valid base64 string");
    }
}

