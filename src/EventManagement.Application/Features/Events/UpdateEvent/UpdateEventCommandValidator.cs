using FluentValidation;

namespace EventManagement.Application.Features.Events.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required for authorization.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Start Date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End Date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End Date cannot be before Start Date.");

        RuleFor(x => x.MaxParticipants)
            .GreaterThan(0).WithMessage("Maximum Participants must be greater than 0.");
    }
}
