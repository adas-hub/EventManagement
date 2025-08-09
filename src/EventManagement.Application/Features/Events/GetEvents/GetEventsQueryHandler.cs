using EventManagement.Application.DTOs.Events;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEvents;

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, Result<PagedResult<EventDto>>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<PagedResult<EventDto>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var pagedEvents = await _eventRepository.GetEventsAsync(
            request.Paging,
            request.Filter,
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