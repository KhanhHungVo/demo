using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Auth.Commands.Login;
using Todo.Application.Auth.Commands.RefreshToken;
using Todo.Application.Auth.Commands.Register;
using Todo.Application.Auth.Commands.RevokeToken;
using Todo.Application.Auth.Queries.GetCurrentUser;

namespace Todo.API.Controllers;

public class AuthController : BaseApiController
{
    private const string RefreshTokenCookie = "refreshToken";

    private CookieOptions RefreshCookieOptions => new()
    {
        HttpOnly = true,
        Secure   = Request.IsHttps,
        SameSite = SameSiteMode.Strict,
        Expires  = DateTimeOffset.UtcNow.AddDays(7),
    };

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand cmd)
    {
        var userId = await Mediator.Send(cmd);
        return CreatedAtAction(nameof(Me), new { }, new { userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand cmd)
    {
        var result = await Mediator.Send(cmd);
        Response.Cookies.Append(RefreshTokenCookie, result.RefreshToken, RefreshCookieOptions);
        return Ok(new { result.AccessToken, result.ExpiresIn });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var token = Request.Cookies[RefreshTokenCookie]
            ?? throw new UnauthorizedAccessException("Refresh token cookie missing.");

        var result = await Mediator.Send(new RefreshTokenCommand(token));
        Response.Cookies.Append(RefreshTokenCookie, result.RefreshToken, RefreshCookieOptions);
        return Ok(new { result.AccessToken, result.ExpiresIn });
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var token = Request.Cookies[RefreshTokenCookie]
            ?? throw new UnauthorizedAccessException("Refresh token cookie missing.");

        await Mediator.Send(new RevokeTokenCommand(token));
        Response.Cookies.Delete(RefreshTokenCookie);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
        => Ok(await Mediator.Send(new GetCurrentUserQuery()));
}
