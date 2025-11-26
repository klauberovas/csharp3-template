namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class DeleteTests : ControllerUnitTestBase
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        //Arrange
        int id = 1;

        RepositoryMock
        .When(async x => await x.DeleteByIdAsync(id))
        .Do(_ => { });

        //Act
        var deleteResult = await Controller.DeleteByIdAsync(id);

        //Asssert
        var noContentResult = Assert.IsType<NoContentResult>(deleteResult);
        Assert.Equal(204, noContentResult.StatusCode);

        await RepositoryMock.Received(1).DeleteByIdAsync(id);
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        //Arrange
        int id = 1;
        RepositoryMock
        .When(async x => await x.DeleteByIdAsync(id))
        .Do(_ => throw new InvalidOperationException());

        //Act
        var deleteResult = await Controller.DeleteByIdAsync(id);

        //Asssert
        Assert.IsType<NotFoundResult>(deleteResult);
        await RepositoryMock.Received(1).DeleteByIdAsync(id);
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        int id = 1;

        RepositoryMock
        .When(async x => await x.DeleteByIdAsync(id))
        .Do(_ => throw new DbUpdateConcurrencyException("Database error"));

        //Act
        var deleteResult = await Controller.DeleteByIdAsync(id);

        //Asssert
        var objectResult = Assert.IsType<ObjectResult>(deleteResult);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);

        await RepositoryMock.Received(1).DeleteByIdAsync(id);
    }
}
