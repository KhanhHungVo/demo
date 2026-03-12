using Todo.Domain.TodoLists;

namespace Todo.Domain.Interfaces;

public interface ITodoListRepository
{
    Task<TodoList?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoList>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(TodoList todoList);
    void Remove(TodoList todoList);
}
