using FluentValidation;

namespace EventManagement.Application.Features.Registrations.UnregisterFromEvent;

public class UnregisterFromEventCommandValidator : AbstractValidator<UnregisterFromEventCommand>
{
	public UnregisterFromEventCommandValidator()
	{
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
