using FluentAssertions;
using Todo.Application.TodoLists.Commands.CreateTodoList;

namespace Todo.Application.Tests.TodoLists;

public class CreateTodoListValidatorTests
{
    private readonly CreateTodoListValidator _validator = new();

    [Fact]
    public void Validate_EmptyTitle_ShouldFail()
    {
        var cmd    = new CreateTodoListCommand("", null);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == nameof(cmd.Title));
    }

    [Fact]
    public void Validate_ValidCommand_ShouldPass()
    {
        var cmd    = new CreateTodoListCommand("My List", "Description");
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }
}
