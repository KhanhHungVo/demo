using MediatR;
using Microsoft.Extensions.Options;
using Todo.Application.Auth.Interfaces;
using Todo.Application.Common.Exceptions;
using Todo.Application.Common.Interfaces;
using Todo.Application.Common.Settings;
using RefreshTokenEntity = Todo.Domain.Entities.RefreshToken;

namespace Todo.Application.Auth.Commands.Login;

// Note: IApplicationDbContext is injected directly here for refresh token persistence.
// In strict Clean Architecture you would introduce IRefreshTokenRepository.
public class LoginHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    IApplicationDbContext db,
    IOptions<JwtSettings> jwtOptions,
    IClientInfoService clientInfo)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand cmd, CancellationToken ct)
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

        var jwt         = jwtOptions.Value;
        var roles       = await identityService.GetRolesAsync(user.Id);
        var accessToken = tokenService.GenerateAccessToken(user.Id, user.Email, roles);
        var rawRefresh  = tokenService.GenerateRefreshToken();

        db.RefreshTokens.Add(new RefreshTokenEntity
        {
            TokenHash   = tokenService.HashToken(rawRefresh),
            UserId      = user.Id,
            ExpiresAt   = DateTime.UtcNow.AddDays(jwt.RefreshTokenExpiryDays),
            CreatedByIp = clientInfo.RemoteIpAddress,
        });
        await db.SaveChangesAsync(ct);

        return new LoginResult(accessToken, rawRefresh, jwt.AccessTokenExpiryMinutes * 60);
    }
}
