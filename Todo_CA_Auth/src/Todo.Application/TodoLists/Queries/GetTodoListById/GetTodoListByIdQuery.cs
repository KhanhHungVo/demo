using MediatR;

namespace Todo.Application.TodoLists.Queries.GetTodoListById;

public record GetTodoListByIdQuery(int Id) : IRequest<TodoListDto>;
