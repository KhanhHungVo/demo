using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Entities;

namespace Todo.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoListCommand, int>
{
    public async Task<int> Handle(CreateTodoListCommand cmd, CancellationToken ct)
    {
        var list = new TodoList
        {
            Title       = cmd.Title.Trim(),
            Description = cmd.Description?.Trim(),
            CreatedAt   = DateTime.UtcNow
        };

        context.TodoLists.Add(list);
        await context.SaveChangesAsync(ct);
        return list.Id;
    }
}
