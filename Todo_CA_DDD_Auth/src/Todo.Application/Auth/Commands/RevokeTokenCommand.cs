using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.Auth.Commands;

public record RevokeTokenCommand(string RefreshToken) : IRequest;

public class RevokeTokenCommandHandler(ITokenService tokenService, IApplicationDbContext db)
    : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand cmd, CancellationToken ct)
    {
        var hash = tokenService.HashToken(cmd.RefreshToken);
        var token = await db.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct)
            ?? throw new UnauthorizedException("Invalid token.");

        if (token.IsRevoked) return;

        token.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
