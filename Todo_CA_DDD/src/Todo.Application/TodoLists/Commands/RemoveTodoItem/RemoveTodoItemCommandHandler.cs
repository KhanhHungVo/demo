using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoLists.Commands.RemoveTodoItem;

public class RemoveTodoItemCommandHandler : IRequestHandler<RemoveTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveTodoItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(RemoveTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == request.TodoListId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoList), request.TodoListId);

        todoList.RemoveItem(request.TodoItemId);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
