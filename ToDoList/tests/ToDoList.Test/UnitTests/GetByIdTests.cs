namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.Models;

public class GetByIdTests : ControllerUnitTestBase
{
    [Fact]
    public void GetById_ItemExists_ReturnsItem()
    {
        //Arrange
        var toDoItem = new ToDoItem() { ToDoItemId = 1, Name = "Task", Description = "Desc", IsCompleted = false };
        RepositoryMock.ReadById(1).Returns(toDoItem);

        //Act
        var result = Controller.ReadById(1);
        var dto = result.GetValue();

        //Assert
        Assert.NotNull(dto);
        Assert.Equal(toDoItem.Name, dto.Name);
        Assert.Equal(toDoItem.Description, dto.Description);
        Assert.Equal(toDoItem.IsCompleted, dto.IsCompleted);
        RepositoryMock.Received(1).ReadById(1);
    }

    [Fact]
    public void GetById_ItemDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        RepositoryMock.ReadById(1).ReturnsNull();

        //Act
        var result = Controller.ReadById(1);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
        RepositoryMock.Received(1).ReadById(1);
    }

    [Fact]
    public void GetById_RepositoryThrowsException_ReturnsProblem500()
    {
        //Arrange
        RepositoryMock
            .When(x => x.ReadById(1))
            .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var result = Controller.ReadById(1);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
