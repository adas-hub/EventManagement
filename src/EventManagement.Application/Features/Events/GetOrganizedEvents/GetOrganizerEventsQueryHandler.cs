using EventManagement.Application.DTOs.Events;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.GetOrganizerEvents;

public class GetOrganizerEventsQueryHandler : IRequestHandler<GetOrganizerEventsQuery, Result<PagedResult<EventDto>>>
{
    private readonly IEventRepository _eventRepository;

    public GetOrganizerEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<PagedResult<EventDto>>> Handle(GetOrganizerEventsQuery request, CancellationToken cancellationToken)
    {
        var filterWithOrganizerId = request.Filter with { OrganizerId = request.OrganizerId };


        var pagedEvents = await _eventRepository.GetEventsAsync(
            request.Paging,
            filterWithOrganizerId,
            request.Sorting,
            cancellationToken);

        var eventDtos = pagedEvents.Items.Select(e => e.ToEventDto()).ToList();

        var result = new PagedResult<EventDto>
        {
            Items = eventDtos,
            TotalCount = pagedEvents.TotalCount,
            PageNumber = pagedEvents.PageNumber,
            PageSize = pagedEvents.PageSize
        };

        return Result<PagedResult<EventDto>>.Success(result);
    }
}