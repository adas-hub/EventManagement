using EventManagement.Application.DTOs.Events;
using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.GetEventDetails;

public record GetEventDetailsQuery(Guid EventId) : IRequest<Result<EventDto>>;
