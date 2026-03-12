using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Enums;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoItems.Commands.UncompleteTodoItem;

public class UncompleteTodoItemHandler(IApplicationDbContext context) : IRequestHandler<UncompleteTodoItemCommand>
{
    public async Task Handle(UncompleteTodoItemCommand cmd, CancellationToken ct)
    {
        var item = await context.TodoItems.FindAsync([cmd.Id], ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoItem), cmd.Id);

        if (item.Status == TodoItemStatus.Pending)
            throw new InvalidOperationException("Item is already pending.");

        item.Status = TodoItemStatus.Pending;
        await context.SaveChangesAsync(ct);
    }
}
