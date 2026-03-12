using Todo.Domain.Common;
using Todo.Domain.Entities;

namespace Todo.Domain.Events;

public class TodoItemAddedEvent : BaseEvent
{
    public TodoList TodoList { get; }
    public TodoItem TodoItem { get; }
    public TodoItemAddedEvent(TodoList todoList, TodoItem todoItem)
    {
        TodoList = todoList;
        TodoItem = todoItem;
    }
}
