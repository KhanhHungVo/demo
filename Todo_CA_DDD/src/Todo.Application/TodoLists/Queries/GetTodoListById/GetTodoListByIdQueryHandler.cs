using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoLists.Queries.GetTodoListById;

public class GetTodoListByIdQueryHandler : IRequestHandler<GetTodoListByIdQuery, TodoListDto>
{
    private readonly IApplicationDbContext _context;

    public GetTodoListByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<TodoListDto> Handle(GetTodoListByIdQuery request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Include(l => l.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoList), request.Id);

        return (TodoListDto)todoList;
    }
}
