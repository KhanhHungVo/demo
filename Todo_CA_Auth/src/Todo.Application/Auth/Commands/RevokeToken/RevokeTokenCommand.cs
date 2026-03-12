using MediatR;

namespace Todo.Application.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string RefreshToken) : IRequest;
