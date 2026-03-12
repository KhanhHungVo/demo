using Todo.Domain.Entities;
using Todo.Domain.Exceptions;
using Todo.Domain.ValueObjects;

namespace Todo.Domain.Tests.TodoLists;

public class TodoListTests
{
    private static TodoList CreateTodoList() => TodoList.Create("Test List", "A test description");

    [Fact]
    public void TodoList_Create_SetsStatusToPending()
        => Assert.Equal(Enums.TodoStatus.Pending, CreateTodoList().Status);

    [Fact]
    public void TodoList_Create_WithEmptyTitle_ThrowsDomainException()
        => Assert.Throws<DomainException>(() => TodoList.Create(""));

    [Fact]
    public void TodoList_AddItem_ChangesStatusToInProgress()
    {
        var list = CreateTodoList();
        list.AddItem("Buy milk", Priority.Medium);
        Assert.Equal(Enums.TodoStatus.InProgress, list.Status);
    }

    [Fact]
    public void TodoList_AddItem_AddsItemToList()
    {
        var list = CreateTodoList();
        list.AddItem("Buy milk", Priority.Medium);
        Assert.Single(list.Items);
        Assert.Equal("Buy milk", list.Items[0].Title);
    }

    [Fact]
    public void TodoList_CompleteItem_WhenAllItemsDone_SetsStatusToCompleted()
    {
        var list = CreateTodoList();
        list.AddItem("Task 1", Priority.Low);
        list.CompleteItem(list.Items[0].Id);
        Assert.Equal(Enums.TodoStatus.Completed, list.Status);
    }

    [Fact]
    public void TodoList_CompleteItem_AlreadyCompleted_ThrowsDomainException()
    {
        var list = CreateTodoList();
        list.AddItem("Task 1", Priority.Low);
        list.CompleteItem(list.Items[0].Id);
        // Item is still in the list (just done), so calling Complete again throws DomainException
        Assert.Throws<DomainException>(() => list.CompleteItem(list.Items[0].Id));
    }

    [Fact]
    public void TodoList_RemoveItem_RemovesItemFromList()
    {
        var list = CreateTodoList();
        list.AddItem("Task 1", Priority.Low);
        var itemId = list.Items[0].Id;
        list.RemoveItem(itemId);
        Assert.Empty(list.Items);
    }

    [Fact]
    public void TodoList_RemoveItem_NotFound_ThrowsNotFoundException()
    {
        var list = CreateTodoList();
        Assert.Throws<NotFoundException>(() => list.RemoveItem(999));
    }

    [Fact]
    public void Priority_WithSameValues_AreEqual()
        => Assert.Equal(Priority.Medium, Priority.Of(1));

    [Fact]
    public void Priority_OutOfRange_ThrowsDomainException()
        => Assert.Throws<DomainException>(() => Priority.Of(99));

    [Fact]
    public void TodoList_Create_RaisesTodoListCreatedEvent()
    {
        var list = CreateTodoList();
        Assert.Single(list.DomainEvents);
        Assert.IsType<Events.TodoListCreatedEvent>(list.DomainEvents.First());
    }
}
