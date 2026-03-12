namespace Todo.Domain.Entities;

public class TodoList
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TodoItem> Items { get; set; } = [];
}
