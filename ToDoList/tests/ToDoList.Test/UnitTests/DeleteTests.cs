namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class DeleteTests : ControllerUnitTestBase
{
    [Fact]
    public void Delete_DeleteByIdValidItemId_ReturnsNoContent()
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
    public void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        //Arrange
        int id = 1;
        RepositoryMock
        .When(x => x.DeleteById(id))
        .Do(_ => throw new InvalidOperationException());

        //Act
        var deleteResult = Controller.DeleteById(id);

        //Asssert
        Assert.IsType<NotFoundResult>(deleteResult);
        RepositoryMock.Received(1).DeleteById(id);
    }

    [Fact]
    public void Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        int id = 1;

        RepositoryMock
        .When(x => x.DeleteById(id))
        .Do(_ => throw new DbUpdateConcurrencyException("Database error"));

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
