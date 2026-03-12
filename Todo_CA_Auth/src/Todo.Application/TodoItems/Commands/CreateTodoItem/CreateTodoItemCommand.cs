using MediatR;

namespace Todo.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand(
    int TodoListId,
    string Title,
    string? Notes,
    DateTime? DueDate) : IRequest<int>;
