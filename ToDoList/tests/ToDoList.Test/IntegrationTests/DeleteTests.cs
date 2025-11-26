namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class DeleteTests : ControllerIntegrationTestBase
{
    [Fact]
    public async Task DeleteByIdExistingItemReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var createdItem = (await Controller.CreateAsync(createRequest)).GetValue()!;

        //Act
        var deleteResult = await Controller.DeleteByIdAsync(createdItem.Id);

        //Assert
        Assert.IsType<NoContentResult>(deleteResult);
    }

    [Fact]
    public async Task DeleteByIdExistingItemRemovesOnlyTarget()
    {
        //Arrange
        var createRequest1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var createRequest2 = new ToDoItemCreateRequestDto("Task2", "Desc12", false);
        var createdItem1 = (await Controller.CreateAsync(createRequest1)).GetValue()!;
        var createdItem2 = (await Controller.CreateAsync(createRequest2)).GetValue()!;

        //Act
        await Controller.DeleteByIdAsync(createdItem1.Id);
        var readResult = await Controller.ReadAsync();
        var remainingItems = readResult.GetValue()!;

        //Assert
        Assert.Single(remainingItems);
        Assert.Equal(createdItem2.Id, remainingItems.Single().Id);
    }

    [Fact]
    public async Task DeleteByIdNonExistingItemReturnsNotFound()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var createdItem = (await Controller.CreateAsync(createRequest)).GetValue()!;
        int nonExistingId = createdItem.Id + 1;

        //Act
        var deleteResult = await Controller.DeleteByIdAsync(nonExistingId);
        var readResult = await Controller.ReadAsync();
        var items = readResult.GetValue();

        //Assert
        Assert.IsType<NotFoundResult>(deleteResult);
        Assert.NotNull(items);
        Assert.Single(items);
        Assert.Equal(createdItem.Id, items.Single().Id);
    }
}
