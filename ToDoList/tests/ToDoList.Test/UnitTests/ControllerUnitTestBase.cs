namespace ToDoList.Test.UnitTests;

using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerUnitTestBase
{
    protected readonly ToDoItemsController Controller;
    protected readonly ToDoItemsContext Context;

    public ControllerUnitTestBase()
    {
        Context = new ToDoItemsContext("Data Source=../../../IntegrationTests/data/localdb_test.db");
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        Controller = new ToDoItemsController(null, repositoryMock);
    }
}
