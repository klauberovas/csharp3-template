namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : ControllerUnitTestBase
{
    [Fact]
    public void Put_ItemExist_ReturnsNoContent()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);
        var existingItem = new ToDoItem { ToDoItemId = 1, Name = "OldTask", Description = "OldDesc", IsCompleted = true };

        RepositoryMock.ReadById(1).Returns(existingItem);
        RepositoryMock
            .When(x => x.Update(Arg.Any<ToDoItem>()))
            .Do(_ => { });


        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        var noContentResult = Assert.IsType<NoContentResult>(updatedResult);
        Assert.Equal(204, noContentResult.StatusCode);

        RepositoryMock.Received(1).ReadById(1);
        RepositoryMock.Received(1).Update(existingItem);
    }

    [Fact]
    public void Put_ItemDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);
        RepositoryMock.ReadById(1).ReturnsNull();

        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updatedResult);
        RepositoryMock.Received(1).ReadById(1);
        RepositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_RepositoryThrowsException_ReturnsProblem500()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);
        var existingItem = new ToDoItem { ToDoItemId = 1, Name = "OldTask", Description = "OldDesc", IsCompleted = true };

        RepositoryMock.ReadById(1).Returns(existingItem);
        RepositoryMock
            .When(x => x.Update(Arg.Any<ToDoItem>()))
            .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(updatedResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
