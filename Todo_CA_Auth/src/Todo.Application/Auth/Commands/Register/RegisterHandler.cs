using FluentValidation.Results;
using MediatR;
using Todo.Application.Auth.Interfaces;
using Todo.Application.Common.Exceptions;

namespace Todo.Application.Auth.Commands.Register;

public class RegisterHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, string>
{
    public async Task<string> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(cmd.Email);
        if (existing is not null)
            throw new ValidationException("Email already registered.");

        var result = await identityService.CreateUserAsync(
            cmd.Email, cmd.Password, cmd.FirstName, cmd.LastName);

        if (!result.Succeeded)
            throw new ValidationException(
                result.Errors.Select(e => new ValidationFailure("", e)));

        var user = await identityService.FindByEmailAsync(cmd.Email);
        return user!.Id;
    }
}
