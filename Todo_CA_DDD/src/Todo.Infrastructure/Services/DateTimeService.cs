using Todo.Application.Common.Interfaces;

namespace Todo.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
