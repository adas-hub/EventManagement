using FluentValidation;

namespace EventManagement.Application.Features.Registrations.RejectRegistration;

public class RejectRegistrationCommandValidator : AbstractValidator<RejectRegistrationCommand>
{
    public RejectRegistrationCommandValidator()
    {
        RuleFor(command => command.RegistrationId)
            .NotEmpty().WithMessage("Registration ID is required to reject a registration.");

        RuleFor(command => command.RejectingUserId)
            .NotEmpty().WithMessage("Rejecting user ID is required.");

        RuleFor(command => command.RejectionReason)
            .MaximumLength(500).When(command => !string.IsNullOrWhiteSpace(command.RejectionReason))
            .WithMessage("Rejection reason cannot exceed 500 characters.");
    }
}