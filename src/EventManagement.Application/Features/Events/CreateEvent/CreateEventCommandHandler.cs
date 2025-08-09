using EventManagement.Domain.Common;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Events.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Result<Guid>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var organizer = await _userRepository.GetByIdAsync(request.OrganizerId, cancellationToken);
        if (organizer == null)
        {
            return Error.Failure("Event.OrganizerNotFound", $"Organizer with ID '{request.OrganizerId}' was not found.");
        }

        var newEvent = new Event
        {
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Location = request.Location,
            MaxParticipants = request.MaxParticipants,
            OrganizerId = request.OrganizerId
        };

        await _eventRepository.AddAsync(newEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(newEvent.Id);
    }
}
