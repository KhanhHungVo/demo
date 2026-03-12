using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Todo.Application.Common.Interfaces;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Domain.Entities;

namespace Todo.Application.Tests.TodoLists;

public class CreateTodoListCommandHandlerTests
{
    private readonly IApplicationDbContext _context = Substitute.For<IApplicationDbContext>();
    private readonly CreateTodoListCommandHandler _handler;

    public CreateTodoListCommandHandlerTests()
        => _handler = new CreateTodoListCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_AddsTodoListAndSaves()
    {
        var todoLists = Substitute.For<DbSet<TodoList>>();
        _context.TodoLists.Returns(todoLists);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var cmd = new CreateTodoListCommand("My List", "My description");
        await _handler.Handle(cmd, CancellationToken.None);

        todoLists.Received(1).Add(Arg.Is<TodoList>(l =>
            l.Title == "My List" && l.Status == Domain.Enums.TodoStatus.Pending));
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
