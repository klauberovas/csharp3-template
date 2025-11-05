namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetTests : ControllerIntegrationTestBase
{

    [Fact]
    public void Get_ItemsExist_ReturnsAllItems()
    {
        //Arrange
        var createRequest1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false);
        var createRequest2 = new ToDoItemCreateRequestDto("Task2", "Desc2", false);

        Controller.Create(createRequest1);
        Controller.Create(createRequest2);

        //Act
        var readResult = Controller.Read();
        var allItems = readResult.GetValue();

        //Assert
        Assert.NotNull(allItems);
        Assert.Equal(2, allItems.Count());

        var firstItem = allItems.FirstOrDefault(i => i.Name == createRequest1.Name);
        var secondItem = allItems.FirstOrDefault(i => i.Name == createRequest2.Name);
        Assert.NotNull(firstItem);
        Assert.NotNull(secondItem);
    }

    [Fact]
    public void Get_NoItems_ReturnsNotFound()
    {
        //Act
        var readResult = Controller.Read();

        //Assert
        Assert.IsType<NotFoundResult>(readResult.Result);
    }
}
