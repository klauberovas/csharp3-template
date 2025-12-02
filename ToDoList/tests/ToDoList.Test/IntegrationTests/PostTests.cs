namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class PostTests : ControllerIntegrationTestBase
{
    [Fact]
    public async Task PostValidRequestReturnsCreatedItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);

        //Act
        var createResult = await Controller.Create(createRequest);

        //Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        Assert.Equal(201, createdAtResult.StatusCode);

        var createdItem = Assert.IsType<ToDoItemGetResponseDto>(createdAtResult.Value);
        Assert.Equal(createRequest.Name, createdItem.Name);
        Assert.Equal(createRequest.Description, createdItem.Description);
        Assert.Equal(createRequest.IsCompleted, createdItem.IsCompleted);
        Assert.Equal(createRequest.Category, createdItem.Category);
    }
}
