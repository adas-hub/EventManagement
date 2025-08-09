using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.CancelEvent;

public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var eventToCancel = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventToCancel == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' was not found.");
        }

        if (eventToCancel.OrganizerId != request.UserId && !request.IsAdmin)
        {
            return Error.Failure("Event.ForbiddenAccess", "Only the event organizer or an admin can cancel this event.");
        }

        if (eventToCancel.Status == EventStatus.Cancelled)
        {
            return Error.Failure("Event.EventAlreadyCancelled", $"Event '{eventToCancel.Title}' is already cancelled.");
        }
        if (eventToCancel.Status == EventStatus.Completed)
        {
            return Error.Failure("Event.EventCannotBeCancelled", $"Event '{eventToCancel.Title}' has already completed and cannot be cancelled.");
        }

        eventToCancel.Status = EventStatus.Cancelled;

        _eventRepository.Update(eventToCancel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
