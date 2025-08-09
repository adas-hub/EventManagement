using EventManagement.Application.DTOs.Users;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using MediatR;

namespace EventManagement.Application.Features.Users.GetUsers;

public record GetUsersQuery(Paging Paging, UserFilter Filter, Sorting Sorting) : IRequest<Result<PagedResult<UserDto>>>;
