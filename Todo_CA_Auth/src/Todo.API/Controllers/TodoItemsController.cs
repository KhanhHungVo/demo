using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.TodoItems.Commands.CompleteTodoItem;
using Todo.Application.TodoItems.Commands.CreateTodoItem;
using Todo.Application.TodoItems.Commands.DeleteTodoItem;
using Todo.Application.TodoItems.Commands.UncompleteTodoItem;
using Todo.Application.TodoItems.Commands.UpdateTodoItem;

namespace Todo.API.Controllers;

public class TodoItemsController : BaseApiController
{
    // POST /api/todoitems → 201
    [HttpPost]
    [Authorize(Policy = "CanWrite")]
    public async Task<ActionResult<int>> Create(CreateTodoItemCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id }, id);
    }

    // PUT /api/todoitems/{id} → 204
    [HttpPut("{id}")]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Update(int id, UpdateTodoItemCommand command)
    {
        await Mediator.Send(command with { Id = id });
        return NoContent();
    }

    // POST /api/todoitems/{id}/complete → 204
    [HttpPost("{id}/complete")]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Complete(int id)
    {
        await Mediator.Send(new CompleteTodoItemCommand(id));
        return NoContent();
    }

    // POST /api/todoitems/{id}/uncomplete → 204
    [HttpPost("{id}/uncomplete")]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Uncomplete(int id)
    {
        await Mediator.Send(new UncompleteTodoItemCommand(id));
        return NoContent();
    }

    // DELETE /api/todoitems/{id} → 204
    [HttpDelete("{id}")]
    [Authorize(Policy = "CanDelete")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoItemCommand(id));
        return NoContent();
    }
}
