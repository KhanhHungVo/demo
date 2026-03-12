using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;

namespace Todo.Infrastructure.Persistence.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly ApplicationDbContext _context;

    public TodoListRepository(ApplicationDbContext context) => _context = context;

    public async Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.TodoLists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TodoLists
            .Include(l => l.Items)
            .ToListAsync(cancellationToken);

    public void Add(TodoList todoList) => _context.TodoLists.Add(todoList);

    public void Remove(TodoList todoList) => _context.TodoLists.Remove(todoList);
}
