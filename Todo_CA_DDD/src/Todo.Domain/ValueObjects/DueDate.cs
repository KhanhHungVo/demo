using Todo.Domain.Common;
using Todo.Domain.Exceptions;

namespace Todo.Domain.ValueObjects;

public class DueDate : ValueObject
{
    public DateTime Value { get; }

    private DueDate(DateTime value) => Value = value;

    public static DueDate Of(DateTime value)
    {
        if (value < DateTime.UtcNow.Date)
            throw new DomainException("Due date cannot be in the past.");
        return new DueDate(value.Date);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
