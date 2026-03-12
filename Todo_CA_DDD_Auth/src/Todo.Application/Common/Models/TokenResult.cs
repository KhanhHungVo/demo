namespace Todo.Application.Common.Models;

public record TokenResult(string AccessToken, string RefreshToken, int ExpiresIn);
