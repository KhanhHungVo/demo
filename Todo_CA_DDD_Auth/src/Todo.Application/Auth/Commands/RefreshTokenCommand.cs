using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Todo.Application.Common.Interfaces;
using Todo.Application.Common.Models;
using Todo.Application.Common.Settings;
using Todo.Domain.Exceptions;
using RefreshTokenEntity = Todo.Domain.Entities.RefreshToken;

namespace Todo.Application.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenResult>;

public class RefreshTokenCommandHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    IApplicationDbContext db,
    IOptions<JwtSettings> jwtOptions)
    : IRequestHandler<RefreshTokenCommand, TokenResult>
{
    public async Task<TokenResult> Handle(RefreshTokenCommand cmd, CancellationToken ct)
    {
        var hash = tokenService.HashToken(cmd.RefreshToken);
        var token = await db.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct)
            ?? throw new UnauthorizedException("Invalid token.");

        if (token.IsRevoked)
        {
            await RevokeAllUserTokensAsync(token.UserId, ct);
            throw new UnauthorizedException("Token reuse detected.");
        }

        if (token.IsExpired)
            throw new UnauthorizedException("Token expired.");

        var jwt = jwtOptions.Value;
        var newRaw = tokenService.GenerateRefreshToken();
        var newHash = tokenService.HashToken(newRaw);

        token.RevokedAt = DateTime.UtcNow;
        token.ReplacedByTokenHash = newHash;

        db.RefreshTokens.Add(new RefreshTokenEntity
        {
            TokenHash = newHash,
            UserId = token.UserId,
            ExpiresAt = DateTime.UtcNow.AddDays(jwt.RefreshTokenExpiryDays),
        });
        await db.SaveChangesAsync(ct);

        var roles = await identityService.GetRolesAsync(token.UserId);
        var userInfo = await identityService.FindByIdAsync(token.UserId)
            ?? throw new UnauthorizedException();
        var accessToken = tokenService.GenerateAccessToken(userInfo.Id, userInfo.Email, roles);

        return new TokenResult(accessToken, newRaw, jwt.ExpiryMinutes * 60);
    }

    private async Task RevokeAllUserTokensAsync(string userId, CancellationToken ct)
    {
        var tokens = await db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToListAsync(ct);
        foreach (var t in tokens) t.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
