using FluentValidation;
using System;
using System.Linq;

namespace EventManagement.Application.Features.Registrations.GetMyRegistrations;

public class GetMyRegistrationsQueryValidator : AbstractValidator<GetMyRegistrationsQuery>
{
    private static readonly string[] AllowedSortFields =
    {
        "registrationdate", "status", "createddate",
        "eventtitle", "eventstartdate", "eventenddate", "eventstatus", "eventlocation"
    };

    public GetMyRegistrationsQueryValidator()
    {
        RuleFor(query => query.UserId)
            .NotEmpty().WithMessage("User ID is required for fetching registrations.");

        RuleFor(query => query.Filter.SearchTerm)
            .MaximumLength(200).When(query => !string.IsNullOrWhiteSpace(query.Filter.SearchTerm))
            .WithMessage("Search term cannot exceed 200 characters.");

        RuleFor(query => query.Sorting.SortBy)
            .Must(BeAValidSortField).When(query => !string.IsNullOrWhiteSpace(query.Sorting.SortBy))
            .WithMessage($"Invalid SortBy field. Allowed values are: {string.Join(", ", AllowedSortFields)}. Case-insensitive.");

        RuleFor(query => query.Sorting.SortOrder)
            .Must(direction =>
                string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase))
            .When(query => !string.IsNullOrWhiteSpace(query.Sorting.SortOrder))
            .WithMessage("Sort order must be 'asc' or 'desc'. Case-insensitive.");
    }

    private bool BeAValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return true;
        return AllowedSortFields.Contains(sortBy.ToLowerInvariant());
    }
}