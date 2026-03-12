using Todo.Domain.Entities;

namespace Todo.Domain.Interfaces;

public interface ITodoListRepository
{
    Task<TodoList?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<TodoList>> GetAllAsync(CancellationToken ct = default);
    void Add(TodoList list);
    void Remove(TodoList list);
}
