using EventManagement.Application.DTOs.Registrations;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using MediatR;

namespace EventManagement.Application.Features.Registrations.GetMyRegistrations;

public record GetMyRegistrationsQuery(
    Guid UserId,
    Paging Paging,
    RegistrationFilter Filter,
    Sorting Sorting
) : IRequest<Result<PagedResult<RegistrationDto>>>;