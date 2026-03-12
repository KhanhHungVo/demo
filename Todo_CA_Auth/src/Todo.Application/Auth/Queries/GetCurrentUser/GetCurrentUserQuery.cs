using MediatR;

namespace Todo.Application.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<CurrentUserDto>;

public record CurrentUserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    IList<string> Roles);
