using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Application.TodoLists.Queries.GetTodoListById;

namespace Todo.Application.TodoLists.Queries.GetAllTodoLists;

public class GetAllTodoListsHandler(IApplicationDbContext context)
    : IRequestHandler<GetAllTodoListsQuery, IEnumerable<TodoListDto>>
{
    public async Task<IEnumerable<TodoListDto>> Handle(GetAllTodoListsQuery query, CancellationToken ct)
    {
        var lists = await context.TodoLists
            .Include(l => l.Items)
            .AsNoTracking()
            .ToListAsync(ct);

        return lists.Select(GetTodoListByIdHandler.MapToDto);
    }
}
