namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

public class DeleteTests : ControllerUnitTestBase
{
    [Fact]
    public void DeleteById_ItemExists_ReturnsNoContent()
    {
        //Arrange
        int id = 1;

        RepositoryMock
        .When(x => x.DeleteById(id))
        .Do(_ => { });

        //Act
        var deleteResult = Controller.DeleteById(id);

        //Asssert
        var noContentResult = Assert.IsType<NoContentResult>(deleteResult);
        Assert.Equal(204, noContentResult.StatusCode);

        RepositoryMock.Received(1).DeleteById(id);
    }

    [Fact]
    public void DeleteById_ItemDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        int id = 1;
        RepositoryMock
        .When(x => x.DeleteById(id))
        .Do(_ => throw new ArgumentOutOfRangeException());

        //Act
        var deleteResult = Controller.DeleteById(id);

        //Asssert
        Assert.IsType<NotFoundResult>(deleteResult);
        RepositoryMock.Received().DeleteById(id);
    }

    [Fact]
    public void DeleteById_AnyItemIdExceptionOccurredDuringDeleteById_ReturnsInternalServerError()
    {
        //Arrange
        int id = 1;

        RepositoryMock
        .When(x => x.DeleteById(id))
        .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var deleteResult = Controller.DeleteById(id);

        //Asssert
        var objectResult = Assert.IsType<ObjectResult>(deleteResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);

        RepositoryMock.Received(1).DeleteById(id);
    }
}
