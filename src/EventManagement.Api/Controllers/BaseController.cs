using EventManagement.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected Guid? UserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdStr &&
                               Guid.TryParse(userIdStr, out var userId) ? userId : null;

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return HandleErrors(result.Errors);
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return HandleErrors(result.Errors);
    }

    private IActionResult HandleErrors(List<Error> errors)
    {
        if (errors == null || !errors.Any())
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected Error",
                Detail = "A failure occurred but no specific error details were provided.",
                Type = $"https://httpstatuses.com/{StatusCodes.Status500InternalServerError}"
            });
        }

        var primaryError = errors.First();
        var (statusCode, title) = GetStatusCodeAndTitle(primaryError.Code);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = primaryError.Message,
            Type = $"https://httpstatuses.com/{statusCode}",
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = errors.Select(e => new { e.Code, e.Message }).ToList()
            }
        };

        return StatusCode(statusCode, problemDetails);
    }

    private static (int statusCode, string title) GetStatusCodeAndTitle(string errorCode)
    {
        return errorCode switch
        {
            var code when code.Contains("NotFound") => (StatusCodes.Status404NotFound, "Resource Not Found"),
            var code when code.Contains("Unauthorized") => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            var code when code.Contains("Forbidden") => (StatusCodes.Status403Forbidden, "Forbidden"),
            var code when code.Contains("Conflict") => (StatusCodes.Status409Conflict, "Conflict"),
            var code when code.Contains("Validation") => (StatusCodes.Status400BadRequest, "Validation Error"),
            _ => (StatusCodes.Status400BadRequest, "Bad Request")
        };
    }
}