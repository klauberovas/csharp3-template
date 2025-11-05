namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerUnitTestBase
{
    protected readonly ToDoItemsController Controller;

    public ControllerUnitTestBase()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(repositoryMock);
    }
}
