using FluentValidation;

namespace EventManagement.Application.Features.Events.GetPublishedEvents;

public class GetPublishedEventsQueryValidator : AbstractValidator<GetPublishedEventsQuery>
{
    public GetPublishedEventsQueryValidator()
    {

        RuleFor(x => x.Paging.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.Paging.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.Filter.SearchTerm)
            .MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Filter.SearchTerm))
            .WithMessage("Search term cannot exceed 200 characters.");

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