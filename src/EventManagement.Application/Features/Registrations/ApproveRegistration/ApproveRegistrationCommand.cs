using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Registrations.ApproveRegistration;

public record ApproveRegistrationCommand(
    Guid RegistrationId,
    Guid ApprovingUserId
) : IRequest<Result>;