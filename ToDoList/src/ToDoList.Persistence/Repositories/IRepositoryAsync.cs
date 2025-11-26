namespace ToDoList.Persistence.Repositories;

public interface IRepositoryAsync<T>
    where T : class
{
    public Task CreateAsync(T item);
    public Task<List<T>> ReadAllAsync();
    public Task<T?> ReadByIdAsync(int id);
    public Task UpdateAsync(T item);
    public Task DeleteByIdAsync(int id);
}
