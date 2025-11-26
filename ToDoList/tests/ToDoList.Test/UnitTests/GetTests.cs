namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;

public class GetTests : ControllerUnitTestBase
{
    [Fact]
    public async Task Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var items = new List<ToDoItem>
        {
            new() {ToDoItemId = 1, Name = "Task1", Description= "Desc1", IsCompleted = false },
            new() {ToDoItemId = 2, Name = "Task2", Description= "Desc2", IsCompleted = false }
        };

        RepositoryMock.ReadAllAsync().Returns(items);

        //Act
        var readResult = await Controller.ReadAsync();
        var allItems = readResult.GetValue();

        //Assert
        Assert.NotNull(allItems);
        Assert.Equal(2, allItems.Count());

        var firstItem = allItems.FirstOrDefault(i => i.Name == "Task1");
        var secondItem = allItems.FirstOrDefault(i => i.Name == "Task2");

        Assert.NotNull(firstItem);
        Assert.NotNull(secondItem);

        await RepositoryMock.Received(1).ReadAllAsync();
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        //Arrange
        RepositoryMock.ReadAllAsync().Returns([]);

        //Act
        var readResult = await Controller.ReadAsync();

        //Assert
        Assert.IsType<NotFoundResult>(readResult.Result);
        await RepositoryMock.Received(1).ReadAllAsync();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        RepositoryMock
            .When(async x => await x.ReadAllAsync())
            .Do(_ => throw new InvalidOperationException("Database error"));

        //Act
        var readResult = await Controller.ReadAsync();

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(readResult.Result);
        Assert.Equal(500, objectResult.StatusCode);

        var problem = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Database error", problem.Detail);
    }
}
