using MediatR;
using Todo.Application.Auth.Commands.Login;

namespace Todo.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResult>;
