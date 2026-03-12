using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoItemCommand>
{
    public async Task Handle(DeleteTodoItemCommand cmd, CancellationToken ct)
    {
        var item = await context.TodoItems.FindAsync([cmd.Id], ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoItem), cmd.Id);

        context.TodoItems.Remove(item);
        await context.SaveChangesAsync(ct);
    }
}
