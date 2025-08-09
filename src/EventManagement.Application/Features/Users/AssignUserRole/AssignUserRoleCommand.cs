using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Users.AssignUserRole;

public record AssignUserRoleCommand(Guid UserId, string RoleName) : IRequest<Result>;
