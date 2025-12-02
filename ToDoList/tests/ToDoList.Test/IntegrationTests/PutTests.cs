namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PutTests : ControllerIntegrationTestBase
{

    [Fact]
    public async Task PutExistingItemReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);
        var updateRequest = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true, "UpdateCategory");

        var createdItem = (await Controller.Create(createRequest)).GetValue()!;

        //Act
        var updateResult = await Controller.UpdateById(createdItem.Id, updateRequest);
        var updatedItem = (await Controller.ReadById(createdItem.Id)).GetValue();

        //Assert
        Assert.IsType<NoContentResult>(updateResult);

        Assert.NotNull(updatedItem);
        Assert.NotEqual(createdItem.Name, updatedItem.Name);
        Assert.Equal(createdItem.Id, updatedItem.Id);
        Assert.Equal(updateRequest.Name, updatedItem.Name);
        Assert.Equal(updateRequest.Description, updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);
        Assert.Equal(updateRequest.Category, updatedItem.Category);
    }

    [Fact]
    public async Task PutNonExistingItemReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false, null);
        int nonExistingId = 999;

        //Act
        var updateResult = await Controller.UpdateById(nonExistingId, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updateResult);
    }

}
