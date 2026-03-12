using MediatR;

namespace Todo.Application.TodoItems.Commands.CompleteTodoItem;

public record CompleteTodoItemCommand(int Id) : IRequest;
