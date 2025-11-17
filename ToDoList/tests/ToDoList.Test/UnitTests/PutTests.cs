namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : ControllerUnitTestBase
{
    [Fact]
    public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);

        RepositoryMock
            .When(x => x.Update(Arg.Any<ToDoItem>()))
            .Do(_ => { });

        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        var noContentResult = Assert.IsType<NoContentResult>(updatedResult);
        Assert.Equal(204, noContentResult.StatusCode);

        RepositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);
        RepositoryMock
        .When(x => x.Update(Arg.Any<ToDoItem>()))
        .Do(_ => throw new InvalidOperationException());

        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updatedResult);

        RepositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false);

        RepositoryMock
            .When(x => x.Update(Arg.Any<ToDoItem>()))
            .Do(_ => throw new DbUpdateConcurrencyException("Database error"));

        //Act
        var updatedResult = Controller.UpdateById(1, updateRequest);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(updatedResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);

        RepositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
    }
}
