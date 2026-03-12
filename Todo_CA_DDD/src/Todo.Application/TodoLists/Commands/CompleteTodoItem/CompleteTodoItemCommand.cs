using MediatR;

namespace Todo.Application.TodoLists.Commands.CompleteTodoItem;

public record CompleteTodoItemCommand(int TodoListId, int TodoItemId) : IRequest;
