using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.Auth.Queries;

public record GetCurrentUserQuery : IRequest<CurrentUserDto>;

public record CurrentUserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles);

public class GetCurrentUserQueryHandler(
    IIdentityService identityService,
    ICurrentUserService currentUser)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery q, CancellationToken ct)
    {
        var userId = currentUser.UserId
            ?? throw new UnauthorizedException();

        var user = await identityService.FindByIdAsync(userId)
            ?? throw new UnauthorizedException();

        var roles = await identityService.GetRolesAsync(userId);

        return new CurrentUserDto(user.Id, user.Email, user.FirstName, user.LastName, roles);
    }
}
