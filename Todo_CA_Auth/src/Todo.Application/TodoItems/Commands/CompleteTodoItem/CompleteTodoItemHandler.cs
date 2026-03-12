using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Enums;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoItems.Commands.CompleteTodoItem;

public class CompleteTodoItemHandler(IApplicationDbContext context) : IRequestHandler<CompleteTodoItemCommand>
{
    public async Task Handle(CompleteTodoItemCommand cmd, CancellationToken ct)
    {
        var item = await context.TodoItems.FindAsync([cmd.Id], ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoItem), cmd.Id);

        if (item.Status == TodoItemStatus.Done)
            throw new InvalidOperationException("Item is already completed.");

        item.Status = TodoItemStatus.Done;
        await context.SaveChangesAsync(ct);
    }
}
