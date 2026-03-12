using Todo.Domain.Enums;

namespace Todo.Application.TodoLists.Queries.GetTodoListById;

public record TodoItemDto(
    int Id,
    string Title,
    string? Notes,
    bool IsDone,
    int Priority,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? CompletedAt
);

public record TodoListDto(
    int Id,
    string Title,
    string? Description,
    TodoStatus Status,
    DateTime CreatedAt,
    List<TodoItemDto> Items
)
{
    public static explicit operator TodoListDto(Domain.Entities.TodoList list) => new(
        list.Id,
        list.Title,
        list.Description,
        list.Status,
        list.CreatedAt,
        [.. list.Items.Select(i => new TodoItemDto(
            i.Id,
            i.Title,
            i.Notes,
            i.IsDone,
            i.Priority.Value,
            i.DueDate?.Value,
            i.CreatedAt,
            i.CompletedAt))]);
}
