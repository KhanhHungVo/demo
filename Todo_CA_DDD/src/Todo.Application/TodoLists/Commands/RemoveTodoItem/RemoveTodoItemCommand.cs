using MediatR;

namespace Todo.Application.TodoLists.Commands.RemoveTodoItem;

public record RemoveTodoItemCommand(int TodoListId, int TodoItemId) : IRequest;
