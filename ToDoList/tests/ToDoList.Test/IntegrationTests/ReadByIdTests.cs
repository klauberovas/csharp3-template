namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class ReadByIdTests : IDisposable
{
    protected readonly ToDoItemsController Controller;
    protected readonly ToDoItemsContext Context;
    public ReadByIdTests()
    {
        Context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        Controller = new ToDoItemsController(Context);
    }

    public void Dispose()
    {
        Context.ToDoItems.RemoveRange(Context.ToDoItems.ToList());
        Context.SaveChanges();
        Context.Dispose();
    }

    [Fact]
    public void ReadById_ExistingItem_ReturnsItem()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var createdItem = Controller.Create(createRequest).GetValue()!;

        //Act
        var readResult = Controller.ReadById(createdItem.Id);
        var foundItem = readResult.GetValue();

        //Assert
        Assert.NotNull(foundItem);
        Assert.Equal(createRequest.Name, foundItem.Name);
        Assert.Equal(createdItem.Id, foundItem.Id);
        Assert.Equal(createRequest.Description, foundItem.Description);
        Assert.Equal(createRequest.IsCompleted, foundItem.IsCompleted);
    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        int nonExistingId = 999;

        //Act
        var readResult = Controller.ReadById(nonExistingId);
        var item = readResult.GetValue();

        //Assert
        Assert.Null(item);
        Assert.IsType<NotFoundResult>(readResult.Result);
    }
}
