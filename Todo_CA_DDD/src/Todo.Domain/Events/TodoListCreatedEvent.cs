using Todo.Domain.Common;
using Todo.Domain.Entities;

namespace Todo.Domain.Events;

public class TodoListCreatedEvent : BaseEvent
{
    public TodoList TodoList { get; }
    public TodoListCreatedEvent(TodoList todoList) => TodoList = todoList;
}
