using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;
using Todo.Domain.TodoLists;

namespace Todo.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand(
    int TodoListId,
    string Title,
    TodoItemPriority Priority = TodoItemPriority.None,
    string? Note = null) : IRequest<int>;

public class CreateTodoItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTodoItemCommand, int>
{
    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == request.TodoListId, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoList), request.TodoListId);

        var item = todoList.AddItem(request.Title, request.Priority, request.Note);
        await context.SaveChangesAsync(cancellationToken);
        return item.Id;
    }
}
