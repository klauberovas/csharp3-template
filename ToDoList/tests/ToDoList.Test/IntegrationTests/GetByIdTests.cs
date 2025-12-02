namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetByIdTests : ControllerIntegrationTestBase
{
    [Fact]
    public async Task GetByIdExistingItemReturnsItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);
        var createdItem = (await Controller.Create(createRequest)).GetValue()!;

        //Act
        var readResult = await Controller.ReadById(createdItem.Id);
        var foundItem = readResult.GetValue();

        //Assert
        Assert.NotNull(foundItem);
        Assert.Equal(createRequest.Name, foundItem.Name);
        Assert.Equal(createdItem.Id, foundItem.Id);
        Assert.Equal(createRequest.Description, foundItem.Description);
        Assert.Equal(createRequest.IsCompleted, foundItem.IsCompleted);
        Assert.Equal(createRequest.Category, foundItem.Category);
    }

    [Fact]
    public async Task GetByIdNonExistingItemReturnsNotFound()
    {
        //Arrange
        int nonExistingId = 999;

        //Act
        var readResult = await Controller.ReadById(nonExistingId);
        var item = readResult.GetValue();

        //Assert
        Assert.Null(item);
        Assert.IsType<NotFoundResult>(readResult.Result);
    }
}
