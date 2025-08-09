using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Registrations.UnregisterFromEvent;

public record UnregisterFromEventCommand(Guid EventId, Guid UserId) : IRequest<Result>;