using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;
using Todo.Domain.ValueObjects;

namespace Todo.Application.TodoLists.Commands.AddTodoItem;

public class AddTodoItemCommandHandler : IRequestHandler<AddTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public AddTodoItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(AddTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == request.TodoListId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoList), request.TodoListId);

        var priority = Priority.Of(request.Priority);
        var dueDate = request.DueDate.HasValue ? DueDate.Of(request.DueDate.Value) : null;

        todoList.AddItem(request.Title, priority, dueDate, request.Notes);
        await _context.SaveChangesAsync(cancellationToken);

        return todoList.Items[^1].Id;
    }
}
