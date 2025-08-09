using EventManagement.Application.DTOs.Events;
using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Events.GetMyOrganizedEvents;

public record GetMyOrganizedEventsQuery(Guid UserId) : IRequest<Result<List<EventDto>>>;