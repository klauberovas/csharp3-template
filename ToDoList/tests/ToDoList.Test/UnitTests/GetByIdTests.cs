namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.Models;

public class GetByIdTests : ControllerUnitTestBase
{
    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var toDoItem = new ToDoItem() { ToDoItemId = 1, Name = "Task", Description = "Desc", IsCompleted = false, Category = null };
        RepositoryMock.ReadByIdAsync(1).Returns(toDoItem);

        //Act
        var readByIdResult = await Controller.ReadById(1);
        var dto = readByIdResult.GetValue();

        //Assert
        Assert.IsType<OkObjectResult>(readByIdResult.Result);
        Assert.NotNull(dto);
        Assert.Equal(toDoItem.Name, dto.Name);
        Assert.Equal(toDoItem.Description, dto.Description);
        Assert.Equal(toDoItem.IsCompleted, dto.IsCompleted);
        Assert.Equal(toDoItem.Category, dto.Category);


        await RepositoryMock.Received(1).ReadByIdAsync(1);
    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        //Arrange
        RepositoryMock.ReadByIdAsync(1).ReturnsNull();

        //Act
        var readByIdResult = await Controller.ReadById(1);

        //Assert
        Assert.IsType<NotFoundResult>(readByIdResult.Result);
        await RepositoryMock.Received(1).ReadByIdAsync(1);
    }

    [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        RepositoryMock
            .When(async x => await x.ReadByIdAsync(1))
            .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var readByIdResult = await Controller.ReadById(1);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(readByIdResult.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
