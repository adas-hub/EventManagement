using FluentValidation;

namespace EventManagement.Application.Features.Registrations.ApproveRegistration;

public class ApproveRegistrationCommandValidator : AbstractValidator<ApproveRegistrationCommand>
{
    public ApproveRegistrationCommandValidator()
    {
        RuleFor(command => command.RegistrationId)
            .NotEmpty().WithMessage("Registration ID is required to approve a registration.");

        RuleFor(command => command.ApprovingUserId)
            .NotEmpty().WithMessage("Approving user ID is required.");
    }
}