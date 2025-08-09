using EventManagement.Application.DTOs.Events;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEventDetails;

public class GetEventDetailsQueryHandler : IRequestHandler<GetEventDetailsQuery, Result<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventDetailsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<EventDto>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdWithOrganizerAsync(request.EventId, cancellationToken);

        if (@event == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' was not found.");
        }

        var eventDetailsDto = @event.ToEventDto();

        return Result<EventDto>.Success(eventDetailsDto);
    }
}
