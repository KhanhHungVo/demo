using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoLists.Queries.GetTodoListById;

public class GetTodoListByIdHandler(IApplicationDbContext context) : IRequestHandler<GetTodoListByIdQuery, TodoListDto>
{
    public async Task<TodoListDto> Handle(GetTodoListByIdQuery query, CancellationToken ct)
    {
        var list = await context.TodoLists
            .Include(l => l.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == query.Id, ct)
            ?? throw new NotFoundException(nameof(TodoList), query.Id);

        return MapToDto(list);
    }

    internal static TodoListDto MapToDto(TodoList list) => new(
        list.Id,
        list.Title,
        list.Description,
        list.CreatedAt,
        list.Items.Select(i => new TodoItemDto(
            i.Id, i.Title, i.Notes, i.Status, i.DueDate, i.CreatedAt)).ToList());
}
