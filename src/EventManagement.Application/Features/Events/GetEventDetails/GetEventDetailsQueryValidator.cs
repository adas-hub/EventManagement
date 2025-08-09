using FluentValidation;

namespace EventManagement.Application.Features.Events.GetEventDetails;

public class GetEventDetailsQueryValidator : AbstractValidator<GetEventDetailsQuery>
{
    public GetEventDetailsQueryValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required.");
    }
}
