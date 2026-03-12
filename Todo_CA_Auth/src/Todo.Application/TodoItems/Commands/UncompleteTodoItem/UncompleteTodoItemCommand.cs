using MediatR;

namespace Todo.Application.TodoItems.Commands.UncompleteTodoItem;

public record UncompleteTodoItemCommand(int Id) : IRequest;
