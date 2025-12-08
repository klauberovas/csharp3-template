namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;
public interface IToDoItemsClient
{
    public Task<ToDoItemView> CreateItemAsync(ToDoItemView toDoItem);
    public Task<List<ToDoItemView>> ReadItemsAsync();
    public Task<ToDoItemView> ReadItemByIdAsync(int itemId);

    public Task UpdateItemAsync(ToDoItemView toDoItem);
    public Task DeleteItemAsync(int itemId);

}
