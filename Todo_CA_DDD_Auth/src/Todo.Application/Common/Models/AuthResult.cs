namespace Todo.Application.Common.Models;

public record AuthResult(bool Succeeded, IEnumerable<string> Errors);
