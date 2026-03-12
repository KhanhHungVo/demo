using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Todo.Application.Common.Interfaces;
using Todo.Application.Common.Models;
using Todo.Application.Common.Settings;
using Todo.Domain.Entities;
using Todo.Domain.Exceptions;

namespace Todo.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<TokenResult>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    IApplicationDbContext db,
    IOptions<JwtSettings> jwtOptions)
    : IRequestHandler<LoginCommand, TokenResult>
{
    public async Task<TokenResult> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var user = await identityService.FindByEmailAsync(cmd.Email)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (await identityService.IsLockedOutAsync(user.Id))
            throw new UnauthorizedException("Account locked. Try again later.");

        if (!await identityService.CheckPasswordAsync(user.Id, cmd.Password))
        {
            await identityService.AccessFailedAsync(user.Id);
            throw new UnauthorizedException("Invalid credentials.");
        }

        await identityService.ResetAccessFailedCountAsync(user.Id);

        var jwt = jwtOptions.Value;
        var roles = await identityService.GetRolesAsync(user.Id);
        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email, roles);
        var rawRefresh = tokenService.GenerateRefreshToken();

        db.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = tokenService.HashToken(rawRefresh),
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(jwt.RefreshTokenExpiryDays),
        });
        await db.SaveChangesAsync(ct);

        return new TokenResult(accessToken, rawRefresh, jwt.ExpiryMinutes * 60);
    }
}
