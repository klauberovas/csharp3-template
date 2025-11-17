namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PostTests : ControllerUnitTestBase
{
    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        RepositoryMock
            .When(x => x.Create(Arg.Any<ToDoItem>()))
            .Do(_ => { });

        //Act
        var createResult = Controller.Create(createRequest);

        //Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        Assert.Equal(201, createdAtResult.StatusCode);

        var createdItem = Assert.IsType<ToDoItemGetResponseDto>(createdAtResult.Value);
        Assert.Equal(createRequest.Name, createdItem.Name);
        Assert.Equal(createRequest.Description, createdItem.Description);
        Assert.Equal(createRequest.IsCompleted, createdItem.IsCompleted);

        RepositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);

        //Mock
        RepositoryMock
            .When(x => x.Create(Arg.Any<ToDoItem>()))
            .Do(_ => throw new InvalidOperationException("Database error"));
        //Act
        var createResult = Controller.Create(createRequest);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(createResult.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
