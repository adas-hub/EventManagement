using FluentValidation;

namespace EventManagement.Application.Features.Users.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Paging.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.Paging.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.Filter.SearchTerm)
            .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Filter.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters.");

        RuleFor(x => x.Sorting.SortBy)
            .Must(BeAValidSortField).When(x => !string.IsNullOrWhiteSpace(x.Sorting.SortBy))
            .WithMessage("Invalid SortBy field. Allowed values are: userName, email, role, createdDate.");
    }

    private bool BeAValidSortField(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return true;

        var validFields = new[] { "username", "email", "createddate" };
        return validFields.Contains(sortBy.ToLower());
    }
}