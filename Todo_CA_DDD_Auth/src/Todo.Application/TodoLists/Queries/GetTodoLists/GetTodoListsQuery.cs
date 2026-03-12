using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;

namespace Todo.Application.TodoLists.Queries.GetTodoLists;

public record GetTodoListsQuery : IRequest<IEnumerable<TodoListDto>>;

public class GetTodoListsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodoListsQuery, IEnumerable<TodoListDto>>
{
    public async Task<IEnumerable<TodoListDto>> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        var lists = await context.TodoLists
            .Include(l => l.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lists.Select(l => (TodoListDto)l);
    }
}
