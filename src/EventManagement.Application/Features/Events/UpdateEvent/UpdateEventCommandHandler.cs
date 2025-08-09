using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdWithOrganizerAsync(request.EventId, cancellationToken);

        if (@event == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' was not found.");
        }

        if (@event.Status != EventStatus.Draft)
        {
            return Error.Failure("Event.Conflict.InvalidStatusForUpdate",
                                  $"Event '{request.EventId}' cannot be updated because its current status is '{@event.Status}'. Only 'Draft' events can be modified.");
        }

        if (!request.IsAdmin && @event.OrganizerId != request.UserId)
        {
            return Error.Failure("Event.Forbidden.UnauthorizedUpdate", "Only the event organizer or an administrator can update this event.");
        }

        @event.Title = request.Title;
        @event.Description = request.Description;
        @event.StartDate = request.StartDate;
        @event.EndDate = request.EndDate;
        @event.Location = request.Location;
        @event.MaxParticipants = request.MaxParticipants;
        @event.UpdatedDate = DateTime.UtcNow;


        _eventRepository.Update(@event);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
