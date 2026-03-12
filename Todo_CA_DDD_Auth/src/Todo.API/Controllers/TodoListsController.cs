using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Application.TodoLists.Commands.DeleteTodoList;
using Todo.Application.TodoLists.Queries.GetTodoLists;

namespace Todo.API.Controllers;

[Authorize]
public class TodoListsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAll()
        => Ok(await Mediator.Send(new GetTodoListsQuery()));

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id }, id);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoListCommand(id));
        return NoContent();
    }
}
