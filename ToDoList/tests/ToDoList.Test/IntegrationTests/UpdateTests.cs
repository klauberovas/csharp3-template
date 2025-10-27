namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.WebApi;

public class UpdateTests : IDisposable
{
    protected readonly ToDoItemsController Controller;
    protected readonly ToDoItemsContext Context;
    public UpdateTests()
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
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var updateRequest = new ToDoItemUpdateRequestDto("UpdateTask", "UpdateDesc", true);

        var createdItem = Controller.Create(createRequest).GetValue()!;

        //Act
        var updateResult = Controller.UpdateById(createdItem.Id, updateRequest);
        var updatedItem = Controller.ReadById(createdItem.Id).GetValue();

        //Assert
        Assert.IsType<NoContentResult>(updateResult);

        Assert.NotNull(updatedItem);
        Assert.NotEqual(createdItem.Name, updatedItem.Name);
        Assert.Equal(createdItem.Id, updatedItem.Id);
        Assert.Equal(updateRequest.Name, updatedItem.Name);
        Assert.Equal(updateRequest.Description, updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);
    }

    [Fact]
    public void UpdateById_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        var updateRequest = new ToDoItemUpdateRequestDto("UpdatedTask", "UpdatedDesc", false);
        int nonExistingId = 999;

        //Act
        var updateResult = Controller.UpdateById(nonExistingId, updateRequest);

        //Assert
        Assert.IsType<NotFoundResult>(updateResult);
    }

}
