namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ToDoList.Domain.Models;

public class DeleteTests : ControllerUnitTestBase
{
    [Fact]
    public void Delete_ItemExists_ReturnsNoContent()
    {
        //Arrange
        var existingItem = new ToDoItem() { ToDoItemId = 1, Name = "Task", Description = "Desc", IsCompleted = true };
        RepositoryMock.ReadById(1).Returns(existingItem);
        RepositoryMock
        .When(x => x.Delete(existingItem))
        .Do(_ => { });

        //Act
        var deleteResult = Controller.DeleteById(1);

        //Asssert
        var noContentResult = Assert.IsType<NoContentResult>(deleteResult);
        Assert.Equal(204, noContentResult.StatusCode);

        RepositoryMock.Received(1).ReadById(1);
        RepositoryMock.Received(1).Delete(existingItem);
    }

    [Fact]
    public void Delete_ItemDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        RepositoryMock.ReadById(1).ReturnsNull();

        //Act
        var deleteResult = Controller.DeleteById(1);

        //Asssert
        Assert.IsType<NotFoundResult>(deleteResult);
        RepositoryMock.Received(1).ReadById(1);
        RepositoryMock.DidNotReceive().Delete(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        //Arrange
        var existingItem = new ToDoItem() { ToDoItemId = 1, Name = "Task", Description = "Desc", IsCompleted = true };
        RepositoryMock.ReadById(1).Returns(existingItem);
        RepositoryMock
        .When(x => x.Delete(existingItem))
        .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var deleteResult = Controller.DeleteById(1);

        //Asssert
        var objectResult = Assert.IsType<ObjectResult>(deleteResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);

        RepositoryMock.Received(1).ReadById(1);
        RepositoryMock.Received(1).Delete(existingItem);
    }

    [Fact]
    public void Delete_AnyItemIdExceptionOccurredDuringReadById_ReturnsInternalServerError()
    {
        //Arrange
        RepositoryMock.ReadById(Arg.Any<int>()).Throws(new Exception());
        int id = 1;

        //Act
        var result = Controller.DeleteById(id);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);

        RepositoryMock.Received(1).ReadById(id);
        RepositoryMock.DidNotReceive().Delete(Arg.Any<ToDoItem>());
    }
}
