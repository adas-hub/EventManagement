using FluentValidation;

namespace EventManagement.Application.Features.Events.GetEvents;

public class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
{
    public GetEventsQueryValidator()
    {
        RuleFor(x => x.Paging.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.Paging.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        // EventFilter Validation (example)
        RuleFor(x => x.Filter.SearchTerm)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Filter.SearchTerm))
            .WithMessage("Search term cannot exceed 200 characters.");

        // Sorting Validation (example)
        RuleFor(x => x.Sorting.SortBy)
            .Must(BeAValidSortField).When(x => !string.IsNullOrWhiteSpace(x.Sorting.SortBy))
            .WithMessage("Invalid SortBy field. Allowed values are: startDate, title, location, maxParticipants, createdDate.");
    }

    private bool BeAValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return true;

        var validFields = new[] { "startdate", "title", "location", "maxparticipants", "createddate" };
        return validFields.Contains(sortBy.ToLower());
    }
}