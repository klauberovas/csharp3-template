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
        var field = typeof(ToDoItemsController).GetField("Items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var items = (List<ToDoItem>)field!.GetValue(null)!;
        items.Clear();
    }

    private ToDoItem AddSampleItem(int id, string name, string description, bool isCompleted)
    {
        var item = new ToDoItem
        {
            ToDoItemId = id,
            Name = name,
            Description = description,
            IsCompleted = isCompleted
        };
        _controller.AddItemToStorage(item);
        return item;
    }

    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItems()
    {
        var request = new ToDoItemCreateRequestDto("Jmeno1", "Popis1", false);

        var result = _controller.Create(request);

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
        var todoItem1 = AddSampleItem(1, "Jmeno1", "Popis1", false);
        var todoItem2 = AddSampleItem(2, "Jmeno2", "Popis2", false);

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
        var todoItem1 = AddSampleItem(1, "Jmeno1", "Popis1", false);

        var result = _controller.ReadById(1);
        var value = result.GetValue();

        Assert.NotNull(value);
        Assert.Equal(value.Id, todoItem1.ToDoItemId);
        Assert.Equal(value.Description, todoItem1.Description);
        Assert.Equal(value.IsCompleted, todoItem1.IsCompleted);
    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {
        var result = _controller.ReadById(2);
        var value = result.GetValue();

        Assert.Null(value);

        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    // ---------- UPDATE ----------
    [Fact]
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {
        var todoItem1 = AddSampleItem(1, "Jmeno1", "Popis1", false);

        var request = new ToDoItemUpdateRequestDto("UpdateName", "UpdateDescription", true);

        var updatedResult = _controller.UpdateById(1, request);
        var result = _controller.ReadById(1);
        var updatedValue = result.GetValue();

        var noContentResult = Assert.IsType<NoContentResult>(updatedResult);
        Assert.Equal(204, noContentResult.StatusCode);

        Assert.NotNull(updatedValue);
        Assert.Equal(todoItem1.ToDoItemId, updatedValue.Id);
        Assert.Equal(request.Name, updatedValue.Name);
        Assert.Equal(request.Description, updatedValue.Description);
        Assert.True(updatedValue.IsCompleted);
    }

    [Fact]
    public void UpdateById_NoExistingItem_ReturnsNotFound()
    {
        AddSampleItem(1, "Jmeno1", "Popis1", false);
        var request = new ToDoItemUpdateRequestDto("UpdatedName", "UpdatedDescription", false);

        var updatedResult = _controller.UpdateById(2, request);

        var notFoundResult = Assert.IsType<NotFoundResult>(updatedResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

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
