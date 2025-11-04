namespace ToDoList.Test.IntegrationTests;

using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerIntegrationTestBase : IDisposable
{
    protected readonly ToDoItemsController Controller;
    protected readonly ToDoItemsContext Context;
    protected readonly ToDoItemsRepository Repository;
    public ControllerIntegrationTestBase()
    {
        string connectingString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        Context = new ToDoItemsContext(connectingString);
        Repository = new ToDoItemsRepository(Context);
        Controller = new ToDoItemsController(Context, Repository);
    }

    public void Dispose()
    {
        Context.ToDoItems.RemoveRange(Context.ToDoItems.ToList());
        Context.SaveChanges();
        Context.Dispose();
    }
}
