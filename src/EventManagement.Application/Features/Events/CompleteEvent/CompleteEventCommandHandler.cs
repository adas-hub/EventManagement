using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.CompleteEvent;

public class CompleteEventCommandHandler : IRequestHandler<CompleteEventCommand, Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventToComplete = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventToComplete == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' was not found.");
        }

        if (eventToComplete.OrganizerId != request.UserId && !request.IsAdmin)
        {
            return Error.Failure("Event.ForbiddenAccess", "Only the event organizer or an admin can complete this event.");
        }

        if (eventToComplete.Status == EventStatus.Completed)
        {
            return Error.Failure("Event.EventAlreadyCompleted", $"Event '{eventToComplete.Title}' is already completed.");
        }

        if (eventToComplete.Status != EventStatus.Published && eventToComplete.Status != EventStatus.Cancelled)
        {
            return Error.Failure("Event.EventCannotBeCompleted", $"Event '{eventToComplete.Title}' is in '{eventToComplete.Status}' status and cannot be manually completed.");
        }

        if (eventToComplete.EndDate > DateTime.UtcNow)
        {
            return Error.Failure("Event.EventCannotBeCompleted", $"Event '{eventToComplete.Title}' cannot be completed as its end date ({eventToComplete.EndDate:yyyy-MM-dd HH:mm}) is still in the future.");
        }

        eventToComplete.Status = EventStatus.Completed;

        _eventRepository.Update(eventToComplete);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
