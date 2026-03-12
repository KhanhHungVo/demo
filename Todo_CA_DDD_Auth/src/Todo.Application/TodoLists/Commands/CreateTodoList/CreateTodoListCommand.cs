using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.TodoLists;

namespace Todo.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand(string Title, string? Colour = null) : IRequest<int>;

public class CreateTodoListCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTodoListCommand, int>
{
    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = TodoList.Create(request.Title, request.Colour);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync(cancellationToken);
        return todoList.Id;
    }
}
