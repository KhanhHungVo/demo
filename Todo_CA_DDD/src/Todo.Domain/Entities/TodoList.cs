using Todo.Domain.Common;
using Todo.Domain.Enums;
using Todo.Domain.Events;
using Todo.Domain.Exceptions;
using Todo.Domain.ValueObjects;

namespace Todo.Domain.Entities;

public class TodoList : AggregateRoot
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<TodoItem> _items = [];
    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    private TodoList() { }

    public static TodoList Create(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo list title is required.");

        var list = new TodoList
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            Status = TodoStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        list.AddDomainEvent(new TodoListCreatedEvent(list));
        return list;
    }

    public void UpdateTitle(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo list title is required.");
        Title = title.Trim();
        Description = description?.Trim();
    }

    public TodoItem AddItem(string title, Priority priority, DueDate? dueDate = null, string? notes = null)
    {
        if (Status == TodoStatus.Completed)
            throw new DomainException("Cannot add items to a completed list.");

        var item = TodoItem.Create(title, priority, dueDate, notes);
        _items.Add(item);
        AddDomainEvent(new TodoItemAddedEvent(this, item));

        if (Status == TodoStatus.Pending)
            Status = TodoStatus.InProgress;

        return item;
    }

    public void CompleteItem(int todoItemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == todoItemId)
            ?? throw new NotFoundException(nameof(TodoItem), todoItemId);

        item.Complete();
        AddDomainEvent(new TodoItemCompletedEvent(this, item));

        if (_items.All(i => i.IsDone))
            Status = TodoStatus.Completed;
    }

    public void RemoveItem(int todoItemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == todoItemId)
            ?? throw new NotFoundException(nameof(TodoItem), todoItemId);

        if (Status == TodoStatus.Completed)
            throw new DomainException("Cannot remove items from a completed list.");

        _items.Remove(item);

        if (_items.Count == 0)
            Status = TodoStatus.Pending;
        else if (_items.All(i => i.IsDone))
            Status = TodoStatus.Completed;
    }
}
