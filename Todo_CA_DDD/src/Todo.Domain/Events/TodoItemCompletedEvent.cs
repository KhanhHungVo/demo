using Todo.Domain.Common;
using Todo.Domain.Entities;

namespace Todo.Domain.Events;

public class TodoItemCompletedEvent : BaseEvent
{
    public TodoList TodoList { get; }
    public TodoItem TodoItem { get; }
    public TodoItemCompletedEvent(TodoList todoList, TodoItem todoItem)
    {
        TodoList = todoList;
        TodoItem = todoItem;
    }
}
