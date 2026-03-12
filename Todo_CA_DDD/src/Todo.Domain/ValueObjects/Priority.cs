using Todo.Domain.Common;
using Todo.Domain.Exceptions;

namespace Todo.Domain.ValueObjects;

public class Priority : ValueObject
{
    public int Value { get; }

    private Priority(int value) => Value = value;

    public static Priority Of(int value)
    {
        if (value < 0 || value > 3)
            throw new DomainException("Priority value must be between 0 (Low) and 3 (Critical).");
        return new Priority(value);
    }

    public static Priority Low => new(0);
    public static Priority Medium => new(1);
    public static Priority High => new(2);
    public static Priority Critical => new(3);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
