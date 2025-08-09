using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.UpdateEvent;

public record UpdateEventCommand(
    Guid EventId,
    Guid UserId,
    bool IsAdmin,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    int MaxParticipants
) : IRequest<Result>;
