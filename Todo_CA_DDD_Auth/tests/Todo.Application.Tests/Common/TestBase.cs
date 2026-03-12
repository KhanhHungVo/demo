using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Todo.Application.Common.Interfaces;

namespace Todo.Application.Tests.Common;

public abstract class TestBase
{
    protected readonly IApplicationDbContext Context;

    protected TestBase()
    {
        Context = Substitute.For<IApplicationDbContext>();
        Context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
    }
}
