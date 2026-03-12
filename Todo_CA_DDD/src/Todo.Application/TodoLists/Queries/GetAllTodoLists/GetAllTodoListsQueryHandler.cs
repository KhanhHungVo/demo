using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Application.TodoLists.Queries.GetTodoListById;

namespace Todo.Application.TodoLists.Queries.GetAllTodoLists;

public class GetAllTodoListsQueryHandler : IRequestHandler<GetAllTodoListsQuery, IEnumerable<TodoListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTodoListsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<TodoListDto>> Handle(GetAllTodoListsQuery request, CancellationToken cancellationToken)
    {
        var lists = await _context.TodoLists
            .Include(l => l.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return lists.Select(l => (TodoListDto)l);
    }
}
