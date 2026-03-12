using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Enums;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoItemCommand, int>
{
    public async Task<int> Handle(CreateTodoItemCommand cmd, CancellationToken ct)
    {
        var list = await context.TodoLists.FindAsync([cmd.TodoListId], ct)
            ?? throw new NotFoundException(nameof(TodoList), cmd.TodoListId);

        var item = new TodoItem
        {
            TodoListId = list.Id,
            Title      = cmd.Title.Trim(),
            Notes      = cmd.Notes?.Trim(),
            Status     = TodoItemStatus.Pending,
            DueDate    = cmd.DueDate,
            CreatedAt  = DateTime.UtcNow
        };

        context.TodoItems.Add(item);
        await context.SaveChangesAsync(ct);
        return item.Id;
    }
}
