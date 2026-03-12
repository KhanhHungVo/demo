using Microsoft.AspNetCore.Identity;
using Todo.Application.Common.Interfaces;

namespace Todo.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<RegisterResult> RegisterAsync(
        string email, string password, string firstName, string lastName)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return new RegisterResult(false, result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, "User");
        return new RegisterResult(true, []);
    }

    public async Task<UserAuthInfo?> FindByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is null ? null : Map(user);
    }

    public async Task<UserAuthInfo?> FindByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? null : Map(user);
    }

    public async Task<bool> CheckPasswordAsync(string userId, string password)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null && await userManager.CheckPasswordAsync(user, password);
    }

    public async Task<bool> IsLockedOutAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null && await userManager.IsLockedOutAsync(user);
    }

    public async Task AccessFailedAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null) await userManager.AccessFailedAsync(user);
    }

    public async Task ResetAccessFailedCountAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null) await userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task<IList<string>> GetRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? [] : await userManager.GetRolesAsync(user);
    }

    private static UserAuthInfo Map(ApplicationUser u) =>
        new(u.Id, u.Email!, u.FirstName, u.LastName);
}
