namespace ToDoList.Test;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ToDoItemsControllerTests : IDisposable
{
    private readonly ToDoItemsController _controller;

    public ToDoItemsControllerTests()
    {
        _controller = new ToDoItemsController();
    }

    public void Dispose()
    {
        // Clear the static list after each test
        var field = typeof(ToDoItemsController).GetField("Items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var items = (List<ToDoItem>)field!.GetValue(null)!;
        items.Clear();
    }

    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItems()
    {
        //Arrange
        var request = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false);

        //Act
        var result = _controller.Create(request);

        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);

        var createdDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(request.Name, createdDto.Name);
        Assert.Equal(request.Description, createdDto.Description);
        Assert.Equal(request.IsCompleted, createdDto.IsCompleted);
        Assert.True(createdDto.Id > 0);
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
        //Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };

        _controller.AddItemToStorage(todoItem1);

        //Act
        var result = _controller.ReadById(1);
        var value = result.GetValue();

        //Assert
        Assert.NotNull(value);
        Assert.Equal(value.Id, todoItem1.ToDoItemId);
        Assert.Equal(value.Description, todoItem1.Description);
        Assert.Equal(value.IsCompleted, todoItem1.IsCompleted);
    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {
        //Act: call ReadById method with a non-existing ID
        var result = _controller.ReadById(2);
        var value = result.GetValue();

        //Assert: check  that no data is returned
        Assert.Null(value);

        //Assert: check the response type is NotFoundResult with status code 404
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    // // ---------- UPDATE ----------
    // [Fact]
    // public void UpdateById_ExistingItem_ReturnsNoContent()
    // {

    // }
    // [Fact]
    // public void UpdateById_NoExistingItem_ReturnsNotFound()
    // {

    // }

    // // -------- DELETE -------
    // [Fact]
    // public void DeleteById_ExistingItem_ReturnsNoContent()
    // {

    // }
    // [Fact]
    // public void DeletById_NoExistingItem_ReturnsNotFound()
    // {

    // }
}
