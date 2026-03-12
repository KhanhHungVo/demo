using Todo.Domain.Common;
using Todo.Domain.Exceptions;
using Todo.Domain.ValueObjects;

namespace Todo.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public bool IsDone { get; private set; }
    public Priority Priority { get; private set; } = null!;
    public DueDate? DueDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private TodoItem() { }

    public static TodoItem Create(string title, Priority priority, DueDate? dueDate = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item title is required.");

        return new TodoItem
        {
            Title = title.Trim(),
            Notes = notes?.Trim(),
            Priority = priority,
            DueDate = dueDate,
            IsDone = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Complete()
    {
        if (IsDone)
            throw new DomainException("Todo item is already completed.");
        IsDone = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Todo item title is required.");
        Title = title.Trim();
    }
}
