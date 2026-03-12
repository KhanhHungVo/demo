using MediatR;
using Todo.Application.Common.Interfaces;
using Todo.Domain.Entities;

namespace Todo.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var todoList = TodoList.Create(request.Title, request.Description);
        _context.TodoLists.Add(todoList);
        await _context.SaveChangesAsync(cancellationToken);
        return todoList.Id;
    }
}
