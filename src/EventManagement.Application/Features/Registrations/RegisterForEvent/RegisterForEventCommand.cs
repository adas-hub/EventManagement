using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Registrations.RegisterForEvent;

public record RegisterForEventCommand(Guid EventId, Guid UserId) : IRequest<Result<Guid>>;
