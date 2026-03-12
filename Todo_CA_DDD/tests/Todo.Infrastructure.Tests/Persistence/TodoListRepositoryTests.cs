using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Entities;
using Todo.Domain.ValueObjects;
using Todo.Infrastructure.Persistence;
using Todo.Infrastructure.Persistence.Repositories;

namespace Todo.Infrastructure.Tests.Persistence;

public class TodoListRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _db;
    private readonly TodoListRepository _repo;

    public TodoListRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _db = new ApplicationDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();
        _repo = new TodoListRepository(_db);
    }

    private static TodoList CreateList() => TodoList.Create("Shopping List", "Weekly groceries");

    [Fact]
    public async Task Add_ThenGetById_ReturnsTodoListWithItems()
    {
        var list = CreateList();
        list.AddItem("Milk", Priority.Low);
        _repo.Add(list);
        await _db.SaveChangesAsync();

        var result = await _repo.GetByIdAsync(list.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Shopping List");
        result.Items.Should().HaveCount(1);
        result.Items[0].Title.Should().Be("Milk");
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
        => (await _repo.GetByIdAsync(999)).Should().BeNull();

    [Fact]
    public async Task GetAllAsync_ReturnsAllLists()
    {
        _repo.Add(CreateList());
        _repo.Add(TodoList.Create("Work Tasks"));
        await _db.SaveChangesAsync();

        var all = await _repo.GetAllAsync();
        all.Should().HaveCount(2);
    }

    [Fact]
    public async Task Remove_DeletesListFromDb()
    {
        var list = CreateList();
        _repo.Add(list);
        await _db.SaveChangesAsync();

        _repo.Remove(list);
        await _db.SaveChangesAsync();

        (await _repo.GetByIdAsync(list.Id)).Should().BeNull();
    }

    public void Dispose() => _db.Dispose();
}
