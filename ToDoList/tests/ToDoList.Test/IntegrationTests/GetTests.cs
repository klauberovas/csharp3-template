namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;

public class GetTests : ControllerIntegrationTestBase
{

    [Fact]
    public async Task GetItemsExistReturnsAllItems()
    {
        //Arrange
        var createRequest1 = new ToDoItemCreateRequestDto("Task1", "Desc1", false, null);
        var createRequest2 = new ToDoItemCreateRequestDto("Task2", "Desc2", false, "category");

        await Controller.Create(createRequest1);
        await Controller.Create(createRequest2);

        //Act
        var readResult = await Controller.Read();
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
    public async Task GetNoItemsReturnsNotFound()
    {
        //Act
        var readResult = await Controller.Read();

        //Assert
        Assert.IsType<NotFoundResult>(readResult.Result);
    }
}
