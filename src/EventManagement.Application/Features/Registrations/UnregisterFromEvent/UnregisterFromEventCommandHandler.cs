using EventManagement.Domain.Common;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.UnregisterFromEvent;

internal class UnregisterFromEventCommandHandler : IRequestHandler<UnregisterFromEventCommand, Result>
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnregisterFromEventCommandHandler(
        IRegistrationRepository registrationRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
    {
        _registrationRepository = registrationRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnregisterFromEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (@event == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' not found.");
        }

        if (@event.EndDate < DateTime.UtcNow)
        {
            return Error.Failure("Event.AlreadyEnded", "Cannot unregister from an event that has already ended.");
        }

        var registration = await _registrationRepository.GetByEventAndUserAsync(request.EventId, request.UserId, cancellationToken);
        if (registration == null)
        {
            return Error.Failure("Registration.NotFound", $"Registration for event '{request.EventId}' by user '{request.UserId}' not found.");
        }

        if (registration.Status == RegistrationStatus.Cancelled || registration.Status == RegistrationStatus.Rejected)
        {
            return Error.Failure("Registration.AlreadyCancelled", "User is already unregistered or their registration was rejected for this event.");
        }

        registration.Status = RegistrationStatus.Cancelled;
        _registrationRepository.Update(registration);

        if (@event.RegisteredParticipantsCount > 0)
        {
            @event.RegisteredParticipantsCount--;
            _eventRepository.Update(@event);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}