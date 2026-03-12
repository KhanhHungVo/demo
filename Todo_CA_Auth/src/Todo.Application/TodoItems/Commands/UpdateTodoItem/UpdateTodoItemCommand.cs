using MediatR;
using Todo.Domain.Enums;

namespace Todo.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(
    int Id,
    string Title,
    string? Notes,
    TodoItemStatus Status,
    DateTime? DueDate) : IRequest;
