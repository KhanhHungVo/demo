using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Auth.Interfaces;
using Todo.Application.Common.Exceptions;
using Todo.Application.Common.Interfaces;

namespace Todo.Application.Auth.Commands.RevokeToken;

public class RevokeTokenHandler(ITokenService tokenService, IApplicationDbContext db)
    : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand cmd, CancellationToken ct)
    {
        var hash  = tokenService.HashToken(cmd.RefreshToken);
        var token = await db.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct)
            ?? throw new UnauthorizedException("Invalid token.");

        if (token.IsRevoked) return;

        token.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
