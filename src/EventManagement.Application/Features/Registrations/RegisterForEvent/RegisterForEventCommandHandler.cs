using EventManagement.Domain.Common;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.RegisterForEvent;

public class RegisterForEventCommandHandler : IRequestHandler<RegisterForEventCommand, Result<Guid>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterForEventCommandHandler(
        IEventRepository eventRepository,
        IUserRepository userRepository,
        IRegistrationRepository registrationRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _registrationRepository = registrationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterForEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdWithRegistrationsAsync(request.EventId, cancellationToken);
        if (@event == null)
        {
            return Error.Failure("Event.NotFound", $"Event with ID '{request.EventId}' not found.");
        }

        if (@event.Status != EventStatus.Published)
        {
            return Error.Failure("Event.NotPublished", "Event is not published and cannot accept registrations.");
        }

        if (@event.StartDate < DateTime.UtcNow)
        {
            return Error.Failure("Event.AlreadyStarted", "Registration for this event has closed as it has already started.");
        }
        if (@event.EndDate < DateTime.UtcNow)
        {
            return Error.Failure("Event.AlreadyEnded", "Registration for this event has closed as it has already ended.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Error.Failure("User.NotFound", $"User with ID '{request.UserId}' not found.");
        }

        var existingRegistration = await _registrationRepository.GetByEventAndUserAsync(request.EventId, request.UserId, cancellationToken);
        if (existingRegistration != null)
        {
            if (existingRegistration.Status != RegistrationStatus.Cancelled && existingRegistration.Status != RegistrationStatus.Rejected)
            {
                return Error.Failure("Registration.AlreadyExists", "User is already registered for this event.");
            }
        }

        var currentParticipants = @event.Registrations.Count(r => r.Status != RegistrationStatus.Cancelled && r.Status != RegistrationStatus.Rejected);
        if (@event.MaxParticipants > 0 && currentParticipants >= @event.MaxParticipants)
        {
            return Error.Failure("Event.Full", "Event has reached its maximum participant capacity.");
        }

        var newRegistration = new Registration
        {
            EventId = request.EventId,
            UserId = request.UserId
        };

        await _registrationRepository.AddAsync(newRegistration);

        @event.RegisteredParticipantsCount++;
        _eventRepository.Update(@event);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(newRegistration.Id);
    }
}