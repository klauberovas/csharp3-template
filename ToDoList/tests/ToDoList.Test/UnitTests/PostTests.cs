namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PostTests : ControllerUnitTestBase
{
    [Fact]
    public async Task Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);
        RepositoryMock
            .When(async x => await x.CreateAsync(Arg.Any<ToDoItem>()))
            .Do(_ => { });

        //Act
        var createResult = await Controller.Create(createRequest);

        //Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        Assert.Equal(201, createdAtResult.StatusCode);

        var createdItem = Assert.IsType<ToDoItemGetResponseDto>(createdAtResult.Value);
        Assert.Equal(createRequest.Name, createdItem.Name);
        Assert.Equal(createRequest.Description, createdItem.Description);
        Assert.Equal(createRequest.IsCompleted, createdItem.IsCompleted);

        await RepositoryMock.Received(1).CreateAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);

        //Mock
        RepositoryMock
            .When(async x => await x.CreateAsync(Arg.Any<ToDoItem>()))
            .Do(_ => throw new InvalidOperationException("Database error"));
        //Act
        var createResult = await Controller.Create(createRequest);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(createResult.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
