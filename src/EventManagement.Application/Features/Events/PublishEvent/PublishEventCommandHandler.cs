using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.PublishEvent;

public class PublishEventCommandHandler : IRequestHandler<PublishEventCommand, Result>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        var eventToPublish = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventToPublish == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' was not found.");
        }

        if (eventToPublish.OrganizerId != request.UserId && !request.IsAdmin)
        {
            return Error.Failure("Event.ForbiddenAccess", "Only the event organizer or an admin can publish this event.");
        }

        if (eventToPublish.Status != EventStatus.Draft)
        {
            return Error.Failure("Event.Conflict.EventAlreadyPublished", $"Event '{eventToPublish.Title}' is already in '{eventToPublish.Status}' status and cannot be published.");
        }

        if (eventToPublish.StartDate < DateTime.UtcNow)
        {
            return Error.Failure("Event.Conflict.EventCannotBePublished", $"Event '{eventToPublish.Title}' cannot be published as its start date is in the past.");
        }

        eventToPublish.Status = EventStatus.Published;

        _eventRepository.Update(eventToPublish);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}