using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Users.RemoveUserRole;

public record RemoveUserRoleCommand(Guid UserId, string RoleName) : IRequest<Result>;
