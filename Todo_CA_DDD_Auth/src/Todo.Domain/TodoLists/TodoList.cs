using Todo.Domain.Common;
using Todo.Domain.Exceptions;
using Todo.Domain.TodoLists.Events;

namespace Todo.Domain.TodoLists;

public class TodoList : BaseAuditableEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Colour { get; private set; }

    private readonly List<TodoItem> _items = [];
    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    private TodoList() { }

    public static TodoList Create(string title, string? colour = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo list title is required.");

        var list = new TodoList
        {
            Title = title,
            Colour = colour,
            CreatedAt = DateTime.UtcNow
        };
        list.AddDomainEvent(new TodoListCreatedEvent(list));
        return list;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo list title is required.");
        Title = title;
        LastModifiedAt = DateTime.UtcNow;
    }

    public TodoItem AddItem(string title, TodoItemPriority priority = TodoItemPriority.None, string? note = null)
    {
        var item = TodoItem.Create(title, priority, note);
        _items.Add(item);
        AddDomainEvent(new TodoItemCreatedEvent(item));
        LastModifiedAt = DateTime.UtcNow;
        return item;
    }

    public void RemoveItem(int todoItemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == todoItemId)
            ?? throw new DomainException($"Todo item {todoItemId} not found.");
        _items.Remove(item);
        LastModifiedAt = DateTime.UtcNow;
    }
}
