namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PutTests : ControllerIntegrationTestBase
{

    [Fact]
    public void Put_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var updateRequest = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true);

        var createdItem = Controller.Create(createRequest).GetValue()!;

        //Act
        var updateResult = Controller.UpdateById(createdItem.Id, updateRequest);
        var updatedItem = Controller.ReadById(createdItem.Id).GetValue();

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
    public void Put_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false);
        int nonExistingId = 999;

        //Act
        var updateResult = Controller.UpdateById(nonExistingId, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updateResult);
    }

}
