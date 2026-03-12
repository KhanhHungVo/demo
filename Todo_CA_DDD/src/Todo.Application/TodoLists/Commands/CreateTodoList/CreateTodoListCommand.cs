using MediatR;

namespace Todo.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand(string Title, string? Description) : IRequest<int>;
