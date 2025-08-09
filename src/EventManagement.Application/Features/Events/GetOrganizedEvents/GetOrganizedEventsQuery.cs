using EventManagement.Application.DTOs.Events;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using MediatR;

namespace EventManagement.Application.Features.Events.GetOrganizerEvents;

public record GetOrganizerEventsQuery(
    Guid OrganizerId,
    Paging Paging,
    EventFilter Filter,
    Sorting Sorting) : IRequest<Result<PagedResult<EventDto>>>;