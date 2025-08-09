using FluentValidation;

namespace EventManagement.Application.Features.Events.GetMyOrganizedEvents;

public class GetMyOrganizedEventsQueryValidator : AbstractValidator<GetMyOrganizedEventsQuery>
{
    public GetMyOrganizedEventsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
