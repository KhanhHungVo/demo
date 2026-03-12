using Todo.Domain.Enums;

namespace Todo.Application.TodoLists.Queries.GetTodoListById;

public record TodoItemDto(int Id, string Title, string? Notes, TodoItemStatus Status, DateTime? DueDate, DateTime CreatedAt);

public record TodoListDto(int Id, string Title, string? Description, DateTime CreatedAt, List<TodoItemDto> Items);
