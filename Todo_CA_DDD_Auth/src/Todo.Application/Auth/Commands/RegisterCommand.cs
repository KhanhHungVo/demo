using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Todo.Application.Common.Interfaces;

namespace Todo.Application.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<string>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}

public class RegisterCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, string>
{
    public async Task<string> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(cmd.Email);
        if (existing is not null)
            throw new Common.Exceptions.ValidationException("Email already registered.");

        var result = await identityService.RegisterAsync(
            cmd.Email, cmd.Password, cmd.FirstName, cmd.LastName);

        if (!result.Succeeded)
            throw new Common.Exceptions.ValidationException(
                result.Errors.Select(e => new ValidationFailure("", e)));

        var user = await identityService.FindByEmailAsync(cmd.Email);
        return user!.Id;
    }
}
