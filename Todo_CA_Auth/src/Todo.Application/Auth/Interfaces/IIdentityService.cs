namespace Todo.Application.Auth.Interfaces;

// Plain DTO records — no Infrastructure types allowed here

public record UserAuthInfo(
    string Id,
    string Email,
    string FirstName,
    string LastName);

public record RegisterResult(bool Succeeded, IEnumerable<string> Errors);

public interface IIdentityService
{
    Task<RegisterResult> CreateUserAsync(
        string email, string password, string firstName, string lastName);

    Task<UserAuthInfo?> FindByEmailAsync(string email);

    Task<UserAuthInfo?> FindByIdAsync(string userId);

    Task<bool> IsLockedOutAsync(string userId);

    Task<bool> CheckPasswordAsync(string userId, string password);

    Task<IList<string>> GetRolesAsync(string userId);

    Task AccessFailedAsync(string userId);

    Task ResetAccessFailedCountAsync(string userId);
}
