using FluentValidation.Results;

namespace Todo.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures occurred.")
        => Errors = failures;

    public ValidationException(string message)
        : base(message)
        => Errors = [];
}
