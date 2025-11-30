namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PutTests : ControllerIntegrationTestBase
{

    [Fact]
    public async Task PutExistingItemReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var updateRequest = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true);

        var createdItem = (await Controller.CreateAsync(createRequest)).GetValue()!;

        //Act
        var updateResult = await Controller.UpdateByIdAsync(createdItem.Id, updateRequest);
        var updatedItem = (await Controller.ReadByIdAsync(createdItem.Id)).GetValue();

        //Assert
        Assert.IsType<NoContentResult>(updateResult);

        Assert.NotNull(updatedItem);
        Assert.NotEqual(createdItem.Name, updatedItem.Name);
        Assert.Equal(createdItem.Id, updatedItem.Id);
        Assert.Equal(updateRequest.Name, updatedItem.Name);
        Assert.Equal(updateRequest.Description, updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);
    }

    [Fact]
    public async Task PutNonExistingItemReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false);
        int nonExistingId = 999;

        //Act
        var updateResult = await Controller.UpdateByIdAsync(nonExistingId, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updateResult);
    }

}
