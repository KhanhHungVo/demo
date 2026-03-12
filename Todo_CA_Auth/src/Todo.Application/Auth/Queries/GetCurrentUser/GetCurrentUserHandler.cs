using MediatR;
using Todo.Application.Auth.Interfaces;
using Todo.Application.Common.Exceptions;
using Todo.Domain.Interfaces;

namespace Todo.Application.Auth.Queries.GetCurrentUser;

public class GetCurrentUserHandler(
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
