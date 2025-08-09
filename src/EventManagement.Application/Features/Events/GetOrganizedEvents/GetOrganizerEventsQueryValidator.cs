using FluentValidation;

namespace EventManagement.Application.Features.Events.GetOrganizerEvents;

public class GetOrganizerEventsQueryValidator : AbstractValidator<GetOrganizerEventsQuery>
{
    private static readonly string[] AllowedSortByValues =
    {
        "startdate", "title", "location", "maxparticipants", "createddate"
    };

    public GetOrganizerEventsQueryValidator()
    {
        RuleFor(x => x.OrganizerId)
            .NotEmpty().WithMessage("OrganizerId cannot be an empty.");

        RuleFor(x => x.Paging.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.Paging.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Sorting.SortOrder)
            .Must(x => string.IsNullOrEmpty(x) || x.ToLower() == "asc" || x.ToLower() == "desc")
            .WithMessage("Sort order must be 'asc' or 'desc'.");

        RuleFor(x => x.Sorting.SortBy)
            .Must(BeAValidSortField)
            .WithMessage($"Invalid SortBy field. Allowed values are: {string.Join(", ", AllowedSortByValues)}.");

        RuleFor(x => x.Filter.SearchTerm)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Filter.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters.");

        RuleFor(x => x.Filter.StartDateFrom)
            .LessThanOrEqualTo(x => x.Filter.StartDateTo)
            .When(x => x.Filter.StartDateFrom.HasValue && x.Filter.StartDateTo.HasValue)
            .WithMessage("Start date from cannot be after start date to.");
    }

    private bool BeAValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return true;
        }
        return AllowedSortByValues.Contains(sortBy.ToLower());
    }
}