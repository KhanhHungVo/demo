namespace Todo.Application.Common.Interfaces;

public record UserAuthInfo(string Id, string Email, string FirstName, string LastName);
public record RegisterResult(bool Succeeded, IEnumerable<string> Errors);

public interface IIdentityService
{
    Task<RegisterResult> RegisterAsync(string email, string password, string firstName, string lastName);
    Task<UserAuthInfo?> FindByEmailAsync(string email);
    Task<UserAuthInfo?> FindByIdAsync(string userId);
    Task<bool> CheckPasswordAsync(string userId, string password);
    Task<bool> IsLockedOutAsync(string userId);
    Task AccessFailedAsync(string userId);
    Task ResetAccessFailedCountAsync(string userId);
    Task<IList<string>> GetRolesAsync(string userId);
}
