namespace ToDoList.Persistence.Repositories;

using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context;
    public ToDoItemsRepository(ToDoItemsContext context) => this.context = context;
    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read() => context.ToDoItems.ToList();

    public ToDoItem? ReadById(int id) => context.ToDoItems.Find(id);

    public void Update(ToDoItem item)
    {
        context.ToDoItems.Update(item);
        context.SaveChanges();
    }

    public void Delete(ToDoItem item)
    {
        context.Remove(item);
        context.SaveChanges();
    }
}
