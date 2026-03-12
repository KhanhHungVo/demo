using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Auth.Commands;
using Todo.Application.Auth.Queries;

namespace Todo.API.Controllers;

public class AuthController : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand cmd)
    {
        var userId = await Mediator.Send(cmd);
        return CreatedAtAction(nameof(Me), new { }, new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand cmd)
        => Ok(await Mediator.Send(cmd));

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand cmd)
        => Ok(await Mediator.Send(cmd));

    [HttpPost("revoke")]
    [Authorize]
    public async Task<IActionResult> Revoke(RevokeTokenCommand cmd)
    {
        await Mediator.Send(cmd);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
        => Ok(await Mediator.Send(new GetCurrentUserQuery()));
}
