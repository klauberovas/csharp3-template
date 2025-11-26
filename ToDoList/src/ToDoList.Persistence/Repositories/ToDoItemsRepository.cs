namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context;
    public ToDoItemsRepository(ToDoItemsContext context) => this.context = context;
    public async Task CreateAsync(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<ToDoItem>> ReadAllAsync() => await context.ToDoItems.ToListAsync();

    public async Task<ToDoItem?> ReadByIdAsync(int id) => await context.ToDoItems.FindAsync(id);

    public async Task UpdateAsync(ToDoItem item)
    {
        var existingItem = await context.ToDoItems.FindAsync(item.ToDoItemId)
        ?? throw new InvalidOperationException($"ToDo item with ID ${item.ToDoItemId} not found.");

        context.Entry(existingItem).CurrentValues.SetValues(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var existingItem = await context.ToDoItems.FindAsync(id)
        ?? throw new InvalidOperationException($"ToDo item with ID {id} is not found.");

        context.ToDoItems.Remove(existingItem);
        await context.SaveChangesAsync();
    }
}
