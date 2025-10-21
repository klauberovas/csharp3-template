namespace ToDoList.Test;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ToDoItemsControllerTests : IDisposable
{
    private readonly ToDoItemsController _controller;

    public ToDoItemsControllerTests(ToDoItemsController _controller)
    {
        this._controller = _controller;
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

    private static void AssertEqualItems(ToDoItem expected, ToDoItemGetResponseDto actual)
    {
        Assert.Equal(expected.ToDoItemId, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.IsCompleted, actual.IsCompleted);
    }

    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItem()
    {
        var request = new ToDoItemCreateRequestDto("Task1", "Desc1", false);

        var actionResult = _controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(201, createdResult.StatusCode);

        var createdDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(request.Name, createdDto.Name);
        Assert.Equal(request.Description, createdDto.Description);
        Assert.Equal(request.IsCompleted, createdDto.IsCompleted);
        Assert.True(createdDto.Id > 0);
    }

    [Fact]
    public void Create_MultipleItems_AssignsIncrementalIds()
    {
        var request1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var request2 = new ToDoItemCreateRequestDto("Task2", "Desc2", false);

        var actionResult1 = _controller.Create(request1);
        var actionResult2 = _controller.Create(request2);

        var dto1 = (actionResult1.Result as CreatedAtActionResult)?.Value as ToDoItemGetResponseDto;
        var dto2 = (actionResult2.Result as CreatedAtActionResult)?.Value as ToDoItemGetResponseDto;

        Assert.Equal(dto1.Id + 1, dto2.Id);
    }

    // ------- READ ALL ------
    [Fact]
    public void Read_ItemsExist_ReturnsAllItems()
    {
        var todoItem1 = AddSampleItem(1, "Task1", "Desc1", false);
        var todoItem2 = AddSampleItem(2, "Task2", "Desc2", false);

        var actionResult = _controller.Read();
        var resultValue = actionResult.GetValue();

        Assert.NotNull(resultValue);

        AssertEqualItems(todoItem1, resultValue.First());
        AssertEqualItems(todoItem2, resultValue.Last());
    }

    [Fact]
    public void Read_NoItems_ReturnsNotFound()
    {
        var actionResult = _controller.Read();
        var resultValue = actionResult.GetValue();

        Assert.IsType<NotFoundResult>(actionResult.Result);
        Assert.Null(resultValue);
    }

    // ---------- READ BY ID ----------
    [Fact]
    public void ReadById_ExistingItem_ReturnsItem()
    {
        var todoItem = AddSampleItem(1, "Task1", "Desc1", false);

        var actionResult = _controller.ReadById(1);
        var resultValue = actionResult.GetValue();

        Assert.NotNull(resultValue);
        AssertEqualItems(todoItem, resultValue);
    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {
        var actionResult = _controller.ReadById(2);
        var resultValue = actionResult.GetValue();

        Assert.Null(resultValue);

        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    // ---------- UPDATE ----------
    [Fact]
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {
        var originaltem = AddSampleItem(1, "Task1", "Desc1", false);
        var request = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true);

        var updateResult = _controller.UpdateById(1, request);
        var readResult = _controller.ReadById(1);
        var resultValue = readResult.GetValue();

        Assert.IsType<NoContentResult>(updateResult);

        Assert.NotNull(resultValue);
        Assert.Equal(originaltem.ToDoItemId, resultValue.Id);
        Assert.Equal(request.Name, resultValue.Name);
        Assert.Equal(request.Description, resultValue.Description);
        Assert.True(resultValue.IsCompleted);
    }

    [Fact]
    public void UpdateById_NonExistingItem_ReturnsNotFound()
    {
        AddSampleItem(1, "Task1", "Desc1", false);
        var request = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false);

        var updateResult = _controller.UpdateById(2, request);

        Assert.IsType<NotFoundResult>(updateResult);
    }

    // -------- DELETE -------
    [Fact]
    public void DeleteById_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        AddSampleItem(1, "Task1", "Desc1", true);

        //Act
        var deleteItem = _controller.DeleteById(1);
        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        //Assert
        Assert.IsType<NoContentResult>(deleteItem);
        Assert.Null(resultValue);
    }

    [Fact]
    public void DeleteById_ExistingItem_RemovesOnlyTarget()
    {
        AddSampleItem(1, "Task1", "Desc1", false);
        AddSampleItem(2, "Task2", "Desc2", false);

        _controller.DeleteById(1);

        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        Assert.Single(resultValue);
        Assert.Equal(2, resultValue.Single().Id);
    }

    [Fact]
    public void DeleteById_NonExistingItem_ReturnsNotFound()
    {
        var existingItem = AddSampleItem(1, "Task1", "Desc1", true);

        var deleteItem = _controller.DeleteById(2);
        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        Assert.IsType<NotFoundResult>(deleteItem);
        Assert.NotNull(resultValue);
        Assert.Single(resultValue);

        var item = resultValue.Single();
        Assert.Equal(existingItem.ToDoItemId, item.Id);
    }
}
