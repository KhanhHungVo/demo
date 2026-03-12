using MediatR;

namespace Todo.Application.TodoLists.Commands.AddTodoItem;

public record AddTodoItemCommand(
    int TodoListId,
    string Title,
    int Priority,
    DateTime? DueDate,
    string? Notes
) : IRequest<int>;
