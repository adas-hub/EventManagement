using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.CancelEvent;

public record CancelEventCommand(Guid EventId, Guid UserId, bool IsAdmin) : IRequest<Result>;
