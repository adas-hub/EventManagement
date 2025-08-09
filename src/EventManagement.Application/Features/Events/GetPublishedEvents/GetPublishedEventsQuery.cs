using EventManagement.Application.DTOs.Events;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using MediatR;

namespace EventManagement.Application.Features.Events.GetPublishedEvents;

public record GetPublishedEventsQuery(
    Paging Paging,
    EventFilter Filter,
    Sorting Sorting
) : IRequest<Result<PagedResult<EventDto>>>;
