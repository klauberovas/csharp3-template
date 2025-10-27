namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class CreateTests : ControllerTestBase
{
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);

        //Act
        var createResult = Controller.Create(createRequest);

        //Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        Assert.Equal(201, createdAtResult.StatusCode);

        var createdItem = Assert.IsType<ToDoItemGetResponseDto>(createdAtResult.Value);
        Assert.Equal(createRequest.Name, createdItem.Name);
        Assert.Equal(createRequest.Description, createdItem.Description);
        Assert.Equal(createRequest.IsCompleted, createdItem.IsCompleted);
    }
}
