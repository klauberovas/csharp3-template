namespace ToDoList.Test.IntegrationTests;

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class ToDoItemsControllerTests : IDisposable
{
    private readonly ToDoItemsController _controller;
    private readonly ToDoItemsContext _context;

    public ToDoItemsControllerTests()
    {
        _context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        _controller = new ToDoItemsController(_context);
    }

    public void Dispose()
    {
        _context.ToDoItems.RemoveRange(_context.ToDoItems);
        _context.SaveChanges();
        _context.Dispose();
    }

    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItem()
    {
        //Arrange
        var request = new ToDoItemCreateRequestDto("Task1", "Desc1", false);

        //Act
        var actionResult = _controller.Create(request);

        //Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(201, createdResult.StatusCode);

        var createdDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(request.Name, createdDto.Name);
        Assert.Equal(request.Description, createdDto.Description);
        Assert.Equal(request.IsCompleted, createdDto.IsCompleted);
    }


    // ------- READ ALL ------
    [Fact]
    public void Read_ItemsExist_ReturnsAllItems()
    {
        //Arrange
        var request1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var request2 = new ToDoItemCreateRequestDto("Task2", "Desc2", false);

        _controller.Create(request1);
        _controller.Create(request2);

        //Act
        var actionResult = _controller.Read();
        var resultValue = actionResult.GetValue();

        //Assert
        Assert.NotNull(resultValue);
        Assert.Equal(2, resultValue.Count());

        var item1 = resultValue.FirstOrDefault(i => i.Name == request1.Name);
        var item2 = resultValue.FirstOrDefault(i => i.Name == request2.Name);
        Assert.NotNull(item1);
        Assert.NotNull(item2);
    }

    [Fact]
    public void Read_NoItems_ReturnsNotFound()
    {
        //Act
        var actionResult = _controller.Read();

        //Assert
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    // ---------- READ BY ID ----------
    [Fact]
    public void ReadById_ExistingItem_ReturnsItem()
    {
        //Arrange
        var requestDto = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var response = _controller.Create(requestDto);
        var responseValue = response.GetValue()!;
        int resultId = responseValue.Id;

        //Act
        var actionResult = _controller.ReadById(resultId);
        var resultValue = actionResult.GetValue();

        //Assert
        Assert.NotNull(resultValue);
        Assert.Equal(requestDto.Name, resultValue.Name);
        Assert.Equal(resultId, resultValue.Id);
        Assert.Equal(requestDto.Description, resultValue.Description);
        Assert.Equal(requestDto.IsCompleted, resultValue.IsCompleted);
    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {
        //Act
        var actionResult = _controller.ReadById(2);
        var resultValue = actionResult.GetValue();

        //Assert
        Assert.Null(resultValue);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    // ---------- UPDATE ----------
    [Fact]
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var originalRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var updateRequest = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true);

        var originalResponse = _controller.Create(originalRequest);
        var originalResponseValue = originalResponse.GetValue();
        int responseId = originalResponseValue!.Id;

        //Act
        var updateResult = _controller.UpdateById(responseId, updateRequest);
        var readResult = _controller.ReadById(responseId);
        var resultValue = readResult.GetValue();

        //Assert
        Assert.IsType<NoContentResult>(updateResult);

        Assert.NotNull(resultValue);
        Assert.NotEqual(originalResponseValue.Name, resultValue.Name);
        Assert.Equal(responseId, resultValue.Id);
        Assert.Equal(updateRequest.Name, resultValue.Name);
        Assert.Equal(updateRequest.Description, resultValue.Description);
        Assert.True(resultValue.IsCompleted);
    }

    [Fact]
    public void UpdateById_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        var request = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false);

        //Act
        var updateResult = _controller.UpdateById(2, request);

        //Assert
        Assert.IsType<NotFoundResult>(updateResult);
    }

    // -------- DELETE -------
    [Fact]
    public void DeleteById_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var request = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var response = _controller.Create(request);
        int responseId = response.GetValue()!.Id;

        //Act
        var deleteItem = _controller.DeleteById(responseId);
        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        //Assert
        Assert.IsType<NoContentResult>(deleteItem);
        Assert.Null(resultValue);
    }

    [Fact]
    public void DeleteById_ExistingItem_RemovesOnlyTarget()
    {
        //Arrange
        var request1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var request2 = new ToDoItemCreateRequestDto("Task2", "Desc12", false);
        var response1 = _controller.Create(request1);
        int responseId1 = response1.GetValue()!.Id;
        var response2 = _controller.Create(request2);
        int responseId2 = response2.GetValue()!.Id;

        //Act
        _controller.DeleteById(responseId1);

        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        //Assert
        Assert.Single(resultValue);
        Assert.Equal(responseId2, resultValue.Single().Id);
    }

    [Fact]
    public void DeleteById_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        var request = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var response = _controller.Create(request);
        int responseId = response.GetValue()!.Id;

        //Act
        var deleteItem = _controller.DeleteById(responseId + 1);
        var readResult = _controller.Read();
        var resultValue = readResult.GetValue();

        //Assert
        Assert.IsType<NotFoundResult>(deleteItem);
        Assert.NotNull(resultValue);
        Assert.Single(resultValue);

        var item = resultValue.Single();
        Assert.Equal(responseId, item.Id);
    }
}
