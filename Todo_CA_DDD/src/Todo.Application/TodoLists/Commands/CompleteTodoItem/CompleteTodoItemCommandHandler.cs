using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;

namespace Todo.Application.TodoLists.Commands.CompleteTodoItem;

public class CompleteTodoItemCommandHandler : IRequestHandler<CompleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public CompleteTodoItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(CompleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == request.TodoListId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TodoList), request.TodoListId);

        todoList.CompleteItem(request.TodoItemId);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
