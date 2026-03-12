using MediatR;

namespace Todo.Application.TodoLists.Queries.GetAllTodoLists;

public record GetAllTodoListsQuery : IRequest<IEnumerable<TodoLists.Queries.GetTodoListById.TodoListDto>>;
