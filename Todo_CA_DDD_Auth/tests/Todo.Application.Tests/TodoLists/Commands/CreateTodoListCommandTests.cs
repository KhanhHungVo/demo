using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Todo.Application.Common.Interfaces;
using Todo.Application.TodoLists.Commands.CreateTodoList;
using Todo.Domain.TodoLists;
using Xunit;

namespace Todo.Application.Tests.TodoLists.Commands;

public class CreateTodoListCommandTests
{
    private readonly IApplicationDbContext _context = Substitute.For<IApplicationDbContext>();
    private readonly CreateTodoListCommandHandler _handler;

    public CreateTodoListCommandTests()
    {
        _handler = new CreateTodoListCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsTodoListAndSaves()
    {
        // Arrange
        var todoLists = Substitute.For<DbSet<TodoList>>();
        _context.TodoLists.Returns(todoLists);
        _context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

        var command = new CreateTodoListCommand("My Todo List", "#FF0000");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        todoLists.Received(1).Add(Arg.Is<TodoList>(l => l.Title == "My Todo List"));
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Validate_EmptyTitle_ShouldFail()
    {
        // Arrange
        var validator = new CreateTodoListCommandValidator();
        var command = new CreateTodoListCommand("");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Title));
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        // Arrange
        var validator = new CreateTodoListCommandValidator();
        var command = new CreateTodoListCommand("My List");

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_TitleExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var validator = new CreateTodoListCommandValidator();
        var command = new CreateTodoListCommand(new string('A', 201));

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(command.Title));
    }
}
