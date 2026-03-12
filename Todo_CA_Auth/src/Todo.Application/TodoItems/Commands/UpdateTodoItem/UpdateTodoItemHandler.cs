using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoItemCommand>
{
    public async Task Handle(UpdateTodoItemCommand cmd, CancellationToken ct)
    {
        var item = await context.TodoItems.FindAsync([cmd.Id], ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoItem), cmd.Id);

        item.Title   = cmd.Title.Trim();
        item.Notes   = cmd.Notes?.Trim();
        item.Status  = cmd.Status;
        item.DueDate = cmd.DueDate;

        await context.SaveChangesAsync(ct);
    }
}
