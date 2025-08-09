using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.CreateEvent
{
    public record CreateEventCommand(
        string Title,
        string Description,
        DateTime StartDate,
        DateTime EndDate,
        string Location,
        int MaxParticipants,
        Guid OrganizerId
    ) : IRequest<Result<Guid>>;
}
