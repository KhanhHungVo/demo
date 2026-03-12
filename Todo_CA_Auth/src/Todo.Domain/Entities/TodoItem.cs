using Todo.Domain.Enums;

namespace Todo.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public TodoItemStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
