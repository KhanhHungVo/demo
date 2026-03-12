using FluentValidation;

namespace Todo.Application.TodoItems.Commands.UncompleteTodoItem;

public class UncompleteTodoItemValidator : AbstractValidator<UncompleteTodoItemCommand>
{
    public UncompleteTodoItemValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
