using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Registrations.RejectRegistration;

public record RejectRegistrationCommand(
    Guid RegistrationId,
    Guid RejectingUserId,
    string? RejectionReason = null 
) : IRequest<Result>;