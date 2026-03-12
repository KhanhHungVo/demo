using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;
using Todo.Domain.TodoLists;

namespace Todo.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(
    int Id,
    string Title,
    string? Note,
    TodoItemPriority Priority,
    bool IsDone) : IRequest;

public class UpdateTodoItemCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateTodoItemCommand>
{
    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var item = await context.TodoItems.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(TodoItem), request.Id);

        item.UpdateTitle(request.Title);
        item.UpdateNote(request.Note);
        item.SetPriority(request.Priority);

        if (request.IsDone && !item.IsDone)
            item.MarkAsDone();
        else if (!request.IsDone && item.IsDone)
            item.MarkAsUndone();

        await context.SaveChangesAsync(cancellationToken);
    }
}
