using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoListCommand>
{
    public async Task Handle(DeleteTodoListCommand cmd, CancellationToken ct)
    {
        var list = await context.TodoLists.FindAsync([cmd.Id], ct)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoList), cmd.Id);

        context.TodoLists.Remove(list);
        await context.SaveChangesAsync(ct);
    }
}
