using EventManagement.Application.Features.Users.AssignUserRole;
using EventManagement.Application.Features.Users.GetUser;
using EventManagement.Application.Features.Users.GetUsers;
using EventManagement.Application.Features.Users.RemoveUserRole;
using EventManagement.Domain.Common;
using EventManagement.Domain.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Api.Controllers;

public class UsersController : BaseController
{
    [HttpPost("{userId:guid}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(Guid userId, AssignUserRoleCommand command)
    {
        if (userId != command.UserId)
        {
            var error = Error.Failure("Validation.UserIdMismatch", "User ID in route must match user ID in the request body.");
            return HandleResult(new List<Error> { error });
        }

        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpDelete("{userId:guid}/roles")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> RemoveRole(Guid userId, [FromBody] RemoveUserRoleCommand command)
    {
        if (userId != command.UserId)
        {
            var error = Error.Failure("Validation.UserIdMismatch", "User ID in route must match user ID in the request body.");
            return HandleResult(new List<Error> { error });
        }

        var result = await Sender.Send(command);
        return HandleResult(result);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var query = new GetUserQuery(userId);
        var result = await Sender.Send(query);
        return HandleResult(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers(
    [FromQuery] Paging paging,
    [FromQuery] UserFilter filter,
    [FromQuery] Sorting sorting)
    {
        var query = new GetUsersQuery(paging, filter, sorting);
        var result = await Sender.Send(query);

        return HandleResult(result);
    }

}