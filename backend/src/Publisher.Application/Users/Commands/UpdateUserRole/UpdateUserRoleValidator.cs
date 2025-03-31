using FluentValidation;
using Publisher.Application.Interfaces;

namespace Publisher.Application.Users.Commands.UpdateUserRole;

public sealed class UpdateUserRoleValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleValidator(ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .Must(role => role == "User" || role == "Admin")
            .WithMessage("Role must be either 'User' or 'Admin'");
            
        RuleFor(x => x)
            .Must((command, _) => 
            {
                var currentUserId = currentUserService.GetCurrentUserId();
                return currentUserId == Guid.Empty || currentUserId != command.UserId;
            })
            .WithMessage("You cannot change your own role");
    }
}
