using Todo.Domain.TodoLists;

namespace Todo.Application.TodoLists.Queries.GetTodoLists;

public record TodoItemBriefDto(int Id, string Title, bool IsDone, TodoItemPriority Priority);

public record TodoListDto(
    int Id,
    string Title,
    string? Colour,
    List<TodoItemBriefDto> Items)
{
    public static explicit operator TodoListDto(TodoList list) => new(
        list.Id,
        list.Title,
        list.Colour,
        [.. list.Items.Select(i => new TodoItemBriefDto(i.Id, i.Title, i.IsDone, i.Priority))]);
}
