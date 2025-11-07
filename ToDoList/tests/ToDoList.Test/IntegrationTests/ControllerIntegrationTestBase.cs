namespace ToDoList.Test.IntegrationTests;

using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
public class ControllerIntegrationTestBase : IDisposable
{
    protected ToDoItemsController Controller { get; }
    protected ToDoItemsContext Context { get; }
    protected ToDoItemsRepository Repository { get; }
    public ControllerIntegrationTestBase()
    {
        string connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db";
        Context = new ToDoItemsContext(connectionString);
        Repository = new ToDoItemsRepository(Context);
        Controller = new ToDoItemsController(Repository);
    }

    public void Dispose()
    {
        Context.ToDoItems.RemoveRange(Context.ToDoItems.ToList());
        Context.SaveChanges();
        Context.Dispose();
    }
}
