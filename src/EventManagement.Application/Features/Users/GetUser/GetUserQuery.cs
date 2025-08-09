using EventManagement.Application.DTOs.Users;
using EventManagement.Domain.Common;
using MediatR;

namespace EventManagement.Application.Features.Users.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<Result<UserDto>>;
