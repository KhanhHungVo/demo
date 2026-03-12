using Todo.Domain.Common;
using Todo.Domain.Exceptions;

namespace Todo.Domain.TodoLists;

public class TodoItem : BaseAuditableEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Note { get; private set; }
    public TodoItemPriority Priority { get; private set; }
    public bool IsDone { get; private set; }

    private TodoItem() { }

    public static TodoItem Create(string title, TodoItemPriority priority = TodoItemPriority.None, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item title is required.");

        return new TodoItem
        {
            Title = title,
            Priority = priority,
            Note = note,
            IsDone = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item title is required.");
        Title = title;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void UpdateNote(string? note)
    {
        Note = note;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void SetPriority(TodoItemPriority priority)
    {
        Priority = priority;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void MarkAsDone()
    {
        if (IsDone) throw new DomainException("Todo item is already done.");
        IsDone = true;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void MarkAsUndone()
    {
        if (!IsDone) throw new DomainException("Todo item is not done.");
        IsDone = false;
        LastModifiedAt = DateTime.UtcNow;
    }
}
