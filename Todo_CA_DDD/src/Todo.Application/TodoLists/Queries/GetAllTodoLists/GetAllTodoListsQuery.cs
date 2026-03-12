using MediatR;
using Todo.Application.TodoLists.Queries.GetTodoListById;

namespace Todo.Application.TodoLists.Queries.GetAllTodoLists;

public record GetAllTodoListsQuery : IRequest<IEnumerable<TodoListDto>>;
