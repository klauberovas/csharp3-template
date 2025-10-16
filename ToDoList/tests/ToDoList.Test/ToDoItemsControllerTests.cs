namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ToDoItemsControllerTests
{
    private readonly ToDoItemsController _controller;

    public ToDoItemsControllerTests()
    {
        _controller = new ToDoItemsController();
    }

    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItems()
    {

    }

    [Fact]
    public void Create_ExceptionThrown_ReturnsProblemResult()
    {

    }

    // ------- READ ALL ------
    [Fact]
    public void Read_ItemsExist_ReturnsAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Jmeno2",
            Description = "Popis2",
            IsCompleted = true
        };

        _controller.AddItemToStorage(todoItem1);
        _controller.AddItemToStorage(todoItem2);

        // Act
        var result = _controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDo.Id);
        Assert.Equal(todoItem1.Name, firstToDo.Name);
        Assert.Equal(todoItem1.Description, firstToDo.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDo.IsCompleted);

        var secondToDo = value.Last();
        Assert.Equal(todoItem2.ToDoItemId, secondToDo.Id);
        Assert.Equal(todoItem2.Name, secondToDo.Name);
        Assert.Equal(todoItem2.Description, secondToDo.Description);
        Assert.Equal(todoItem2.IsCompleted, secondToDo.IsCompleted);
    }

    [Fact]
    public void Read_NoItems_ReturnsNotFound()
    {
        var result = _controller.Read();
        var value = result.GetValue();
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);

        Assert.Null(value);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    // ---------- READ BY ID ----------
    [Fact]
    public void ReadById_ExistingItem_ReturnsItem()
    {

    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {

    }

    // ---------- UPDATE ----------
    [Fact]
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {

    }
    [Fact]
    public void UpdateById_NoExistingItem_ReturnsNotFound()
    {

    }

    [Fact]
    public void UpdateById_ExceptionThrown_ReturnsProblem()
    {

    }

    // -------- DELETE -------
    [Fact]
    public void DeleteById_ExistingItem_ReturnsNoContent()
    {

    }
    [Fact]
    public void DeletById_NoExistingItem_ReturnsNotFound()
    {

    }

    [Fact]
    public void DeleteById_ExceptionThrown_ReturnsProblem()
    {

    }
}
