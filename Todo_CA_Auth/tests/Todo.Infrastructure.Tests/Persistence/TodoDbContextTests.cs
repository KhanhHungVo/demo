using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.Enums;
using Todo.Infrastructure.Persistence;

namespace Todo.Infrastructure.Tests.Persistence;

public class TodoDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _db;

    public TodoDbContextTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _db = new ApplicationDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
    }

    [Fact]
    public async Task SaveTodoList_ThenQueryById_ReturnsTodoList()
    {
        var list = new TodoList
        {
            Title     = "Work",
            CreatedAt = DateTime.UtcNow
        };
        _db.TodoLists.Add(list);
        await _db.SaveChangesAsync();

        var result = await _db.TodoLists.FindAsync(list.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Work");
    }

    [Fact]
    public async Task SaveTodoItem_ThenQueryById_ReturnsTodoItem()
    {
        var list = new TodoList { Title = "Work", CreatedAt = DateTime.UtcNow };
        _db.TodoLists.Add(list);
        await _db.SaveChangesAsync();

        var item = new TodoItem
        {
            TodoListId = list.Id,
            Title      = "Task 1",
            Status     = TodoItemStatus.Pending,
            CreatedAt  = DateTime.UtcNow
        };
        _db.TodoItems.Add(item);
        await _db.SaveChangesAsync();

        var result = await _db.TodoItems.FindAsync(item.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Task 1");
        result.Status.Should().Be(TodoItemStatus.Pending);
    }

    public void Dispose() => _db.Dispose();
}
