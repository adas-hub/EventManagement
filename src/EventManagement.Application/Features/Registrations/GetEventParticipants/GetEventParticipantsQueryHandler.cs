using EventManagement.Application.DTOs.Registrations;
using EventManagement.Application.Mappers;
using EventManagement.Domain.Common;
using EventManagement.Domain.Repositories;
using MediatR;

namespace EventManagement.Application.Features.Registrations.GetEventParticipants;

public class GetEventParticipantsQueryHandler : IRequestHandler<GetEventParticipantsQuery, Result<PagedResult<RegistrationDto>>>
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IEventRepository _eventRepository;

    public GetEventParticipantsQueryHandler(IRegistrationRepository registrationRepository, IEventRepository eventRepository)
    {
        _registrationRepository = registrationRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Result<PagedResult<RegistrationDto>>> Handle(GetEventParticipantsQuery request, CancellationToken cancellationToken)
    {
        var eventExists = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken);
        if (eventExists == null)
        {
            return Error.Failure("Event.NotFound",$"Event with ID '{request.EventId}' not found.");
        }
        var pagedRegistrations = await _registrationRepository.GetEventRegistrationsAsync(
            request.EventId,
            request.Paging,
            request.Filter,
            request.Sorting,
            cancellationToken);

        var registrationDtos = pagedRegistrations.Items.Select(r => r.ToRegistrationDto()).ToList();

        var result = new PagedResult<RegistrationDto>
        {
            Items = registrationDtos,
            TotalCount = pagedRegistrations.TotalCount,
            PageNumber = pagedRegistrations.PageNumber,
            PageSize = pagedRegistrations.PageSize
        };

        return Result<PagedResult<RegistrationDto>>.Success(result);
    }
}