using Microsoft.AspNetCore.Mvc;
using Todo.Application.TodoLists.Commands.AddTodoItem;
using Todo.Application.TodoLists.Commands.CompleteTodoItem;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Application.TodoLists.Commands.DeleteTodoList;
using Todo.Application.TodoLists.Commands.RemoveTodoItem;
using Todo.Application.TodoLists.Queries.GetAllTodoLists;
using Todo.Application.TodoLists.Queries.GetTodoListById;

namespace Todo.API.Controllers;

public class TodoListsController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoListDto>>> GetAll()
        => Ok(await Mediator.Send(new GetAllTodoListsQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoListDto>> GetById(int id)
        => Ok(await Mediator.Send(new GetTodoListByIdQuery(id)));

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoListCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoListCommand(id));
        return NoContent();
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<int>> AddItem(int id, AddTodoItemCommand command)
    {
        var itemId = await Mediator.Send(command with { TodoListId = id });
        return CreatedAtAction(nameof(GetById), new { id }, itemId);
    }

    [HttpPost("{id}/items/{itemId}/complete")]
    public async Task<IActionResult> CompleteItem(int id, int itemId)
    {
        await Mediator.Send(new CompleteTodoItemCommand(id, itemId));
        return NoContent();
    }

    [HttpDelete("{id}/items/{itemId}")]
    public async Task<IActionResult> RemoveItem(int id, int itemId)
    {
        await Mediator.Send(new RemoveTodoItemCommand(id, itemId));
        return NoContent();
    }
}
