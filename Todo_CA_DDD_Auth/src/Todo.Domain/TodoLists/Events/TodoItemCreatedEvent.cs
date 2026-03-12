using Todo.Domain.Common;

namespace Todo.Domain.TodoLists.Events;

public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItem TodoItem { get; }
    public TodoItemCreatedEvent(TodoItem todoItem) => TodoItem = todoItem;
}
