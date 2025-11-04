namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class DeleteTests : ControllerIntegrationTestBase
{
    [Fact]
    public void DeleteById_ExistingItem_ReturnsNoContent()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var createdItem = Controller.Create(createRequest).GetValue()!;

        //Act
        var deleteResult = Controller.DeleteById(createdItem.Id);
        var readResult = Controller.Read();
        var items = readResult.GetValue();

        //Assert
        Assert.IsType<NoContentResult>(deleteResult);
        Assert.Null(items);
    }

    [Fact]
    public void DeleteById_ExistingItem_RemovesOnlyTarget()
    {
        //Arrange
        var createRequest1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var createRequest2 = new ToDoItemCreateRequestDto("Task2", "Desc12", false);
        var createdItem1 = Controller.Create(createRequest1).GetValue()!;
        var createdItem2 = Controller.Create(createRequest2).GetValue()!;

        //Act
        Controller.DeleteById(createdItem1.Id);
        var readResult = Controller.Read();
        var remainingItems = readResult.GetValue();

        //Assert
        Assert.Single(remainingItems);
        Assert.Equal(createdItem2.Id, remainingItems.Single().Id);
    }

    [Fact]
    public void DeleteById_NonExistingItem_ReturnsNotFound()
    {
        //Arrange
        var createRequest = new ToDoItemCreateRequestDto("Task1", "Desc1", true);
        var createdItem = Controller.Create(createRequest).GetValue()!;
        int nonExistingId = createdItem.Id + 1;

        //Act
        var deleteResult = Controller.DeleteById(nonExistingId);
        var readResult = Controller.Read();
        var items = readResult.GetValue();

        //Assert
        Assert.IsType<NotFoundResult>(deleteResult);
        Assert.NotNull(items);
        Assert.Single(items);
        Assert.Equal(createdItem.Id, items.Single().Id);
    }
}
