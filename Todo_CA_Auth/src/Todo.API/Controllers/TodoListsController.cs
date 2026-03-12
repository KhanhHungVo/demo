using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Application.TodoLists.Commands.DeleteTodoList;
using Todo.Application.TodoLists.Queries.GetAllTodoLists;
using Todo.Application.TodoLists.Queries.GetTodoListById;

namespace Todo.API.Controllers;

public class TodoListsController : BaseApiController
{
    // GET /api/todolists
    [HttpGet]
    [Authorize(Policy = "CanRead")]
    public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAll()
        => Ok(await Mediator.Send(new GetAllTodoListsQuery()));

    // GET /api/todolists/{id}
    [HttpGet("{id}")]
    [Authorize(Policy = "CanRead")]
    public async Task<ActionResult<TodoListDto>> GetById(int id)
        => Ok(await Mediator.Send(new GetTodoListByIdQuery(id)));

    // POST /api/todolists → 201
    [HttpPost]
    [Authorize(Policy = "CanWrite")]
    public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    // DELETE /api/todolists/{id} → 204
    [HttpDelete("{id}")]
    [Authorize(Policy = "CanDelete")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoListCommand(id));
        return NoContent();
    }
}
