namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetByIdTests : ControllerIntegrationTestBase
{
    [Fact]
    public void GetByIdExistingItemReturnsItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var createdItem = Controller.Create(createRequest).GetValue()!;

        //Act
        var readResult = Controller.ReadById(createdItem.Id);
        var foundItem = readResult.GetValue();

        //Assert
        Assert.NotNull(foundItem);
        Assert.Equal(createRequest.Name, foundItem.Name);
        Assert.Equal(createdItem.Id, foundItem.Id);
        Assert.Equal(createRequest.Description, foundItem.Description);
        Assert.Equal(createRequest.IsCompleted, foundItem.IsCompleted);
    }

    [Fact]
    public void GetByIdNonExistingItemReturnsNotFound()
    {
        //Arrange
        int nonExistingId = 999;

        //Act
        var readResult = Controller.ReadById(nonExistingId);
        var item = readResult.GetValue();

        //Assert
        Assert.Null(item);
        Assert.IsType<NotFoundResult>(readResult.Result);
    }
}
