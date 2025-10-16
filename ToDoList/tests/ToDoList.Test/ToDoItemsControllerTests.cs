namespace ToDoList.Test;

using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ToDoItemsControllerTests
{
    // ------- CREATE ------
    [Fact]
    public void Create_ValidRequest_ReturnsCreatedItems()
    {

    }

    [Fact]
    public void Create_ExceptionThrown_ReturnProblemResult()
    {

    }

    // ------- READ ALL ------
    [Fact]
    public void Read_ItemsExist_ReturnAllItems()
    {
        // Arrange
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno1",
            Description = "Popis1",
            IsCompleted = false
        };
        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Jmeno2",
            Description = "Popis2",
            IsCompleted = true
        };

        var controller = new ToDoItemsController();
        controller.AddItemToStorage(todoItem1);
        controller.AddItemToStorage(todoItem2);

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDo.Id);
        Assert.Equal(todoItem1.Name, firstToDo.Name);
        Assert.Equal(todoItem1.Description, firstToDo.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDo.IsCompleted);

        var secondToDo = value.Last();
        Assert.Equal(todoItem2.ToDoItemId, secondToDo.Id);
        Assert.Equal(todoItem2.Name, secondToDo.Name);
        Assert.Equal(todoItem2.Description, secondToDo.Description);
        Assert.Equal(todoItem2.IsCompleted, secondToDo.IsCompleted);
    }

    [Fact]
    public void Read_NoItems_ReturnsNotFound()
    {

    }

    // ---------- READ BY ID ----------
    [Fact]
    public void ReadById_ExistingItem_ReturnsItem()
    {

    }

    [Fact]
    public void ReadById_NonExistingItem_ReturnsNotFound()
    {

    }

    // ---------- UPDATE ----------
    [Fact]
    public void UpdateById_ExistingItem_ReturnsNoContent()
    {

    }
    [Fact]
    public void UpdateById_NoExistingItem_ReturnsNotFound()
    {

    }

    [Fact]
    public void UpdateById_ExceptionThrown_ReturnsProblem()
    {

    }

    // -------- DELETE -------
    [Fact]
    public void DeleteById_ExistingItem_ReturnsNoContent()
    {

    }
    [Fact]
    public void DeletById_NoExistingItem_ReturnsNotFound()
    {

    }

    [Fact]
    public void DeleteById_ExceptionThrown_ReturnsProblem()
    {

    }
}
