using NSubstitute;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Common.Interfaces;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Domain.Entities;

namespace Todo.Application.Tests.TodoLists;

public class CreateTodoListHandlerTests
{
    private readonly IApplicationDbContext _context = Substitute.For<IApplicationDbContext>();
    private readonly CreateTodoListHandler _handler;

    public CreateTodoListHandlerTests() => _handler = new CreateTodoListHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_AddsTodoListAndReturnsId()
    {
        var lists = Substitute.For<DbSet<TodoList>>();
        _context.TodoLists.Returns(lists);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var cmd = new CreateTodoListCommand("My List", "A description");

        await _handler.Handle(cmd, CancellationToken.None);

        lists.Received(1).Add(Arg.Is<TodoList>(l => l.Title == "My List"));
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
