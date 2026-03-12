using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Exceptions;
using Todo.Domain.TodoLists;

namespace Todo.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteTodoListCommand>
{
    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = await context.TodoLists.FindAsync([request.Id], cancellationToken)
            ?? throw new NotFoundException(nameof(TodoList), request.Id);

        context.TodoLists.Remove(todoList);
        await context.SaveChangesAsync(cancellationToken);
    }
}
