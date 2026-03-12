using Todo.Domain.Common;

namespace Todo.Domain.TodoLists.Events;

public class TodoListCreatedEvent : BaseEvent
{
    public TodoList TodoList { get; }
    public TodoListCreatedEvent(TodoList todoList) => TodoList = todoList;
}
