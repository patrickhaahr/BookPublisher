using FluentValidation;
using Publisher.Application.Utils;

namespace Publisher.Application.Covers.Commands.CreateCover;

public class CreateCoverCommandValidator : AbstractValidator<CreateCoverCommand>
{
    public CreateCoverCommandValidator()
    {
        RuleFor(c => c.ImgBase64)
            .NotEmpty()
            .Must(Validation.IsValidBase64Image)
            .WithMessage("Image base64 must be a valid base64 string");
    }
}

