using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.PublishEvent;

public record PublishEventCommand(Guid EventId, Guid UserId, bool IsAdmin) : IRequest<Result>;
