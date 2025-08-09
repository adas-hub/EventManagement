using EventManagement.Application.Features.Auth.Login;
using EventManagement.Application.Features.Auth.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Api.Controllers;
public class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Sender.Send(command);

        return HandleResult(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await Sender.Send(command);

        return HandleResult(result);
    }
}