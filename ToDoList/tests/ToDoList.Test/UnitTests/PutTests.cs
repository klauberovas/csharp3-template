namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class PutTests : ControllerUnitTestBase
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false, "Category1");

        RepositoryMock
            .When(async x => await x.UpdateAsync(Arg.Any<ToDoItem>()))
            .Do(_ => { });

        //Act
        var updatedResult = await Controller.UpdateById(1, updateRequest);

        //Assert
        var noContentResult = Assert.IsType<NoContentResult>(updatedResult);
        Assert.Equal(204, noContentResult.StatusCode);

        await RepositoryMock.Received(1).UpdateAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false, null);
        RepositoryMock
        .When(async x => await x.UpdateAsync(Arg.Any<ToDoItem>()))
        .Do(_ => throw new InvalidOperationException());

        //Act
        var updatedResult = await Controller.UpdateById(1, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updatedResult);

        await RepositoryMock.Received(1).UpdateAsync(Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("Task1", "Desc1", false, null);

        RepositoryMock
            .When(async x => await x.UpdateAsync(Arg.Any<ToDoItem>()))
            .Do(_ => throw new DbUpdateConcurrencyException("Database error"));

        //Act
        var updatedResult = await Controller.UpdateById(1, updateRequest);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(updatedResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);

        await RepositoryMock.Received(1).UpdateAsync(Arg.Any<ToDoItem>());
    }
}
