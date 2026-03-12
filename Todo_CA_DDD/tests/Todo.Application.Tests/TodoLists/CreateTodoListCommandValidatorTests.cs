using FluentAssertions;
using Todo.Application.TodoLists.Commands.CreateTodoList;

namespace Todo.Application.Tests.TodoLists;

public class CreateTodoListCommandValidatorTests
{
    private readonly CreateTodoListCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyTitle_ShouldFail()
    {
        var cmd = new CreateTodoListCommand("", null);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.Title));
    }

    [Fact]
    public void Validate_TitleTooLong_ShouldFail()
    {
        var cmd = new CreateTodoListCommand(new string('A', 201), null);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        var cmd = new CreateTodoListCommand("Buy groceries", "Weekly shopping list");
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }
}
