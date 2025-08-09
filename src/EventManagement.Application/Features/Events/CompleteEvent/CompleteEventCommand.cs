using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.CompleteEvent;

public record CompleteEventCommand(Guid EventId, Guid UserId, bool IsAdmin) : IRequest<Result>;
