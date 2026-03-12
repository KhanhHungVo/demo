using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.TodoItems.Commands.CreateTodoItem;
using Todo.Application.TodoItems.Commands.UpdateTodoItem;

namespace Todo.API.Controllers;

[Authorize]
public class TodoItemsController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoItemCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(null, new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTodoItemCommand command)
    {
        await Mediator.Send(command with { Id = id });
        return NoContent();
    }
}
