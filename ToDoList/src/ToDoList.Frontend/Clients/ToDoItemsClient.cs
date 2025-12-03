namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;
public class ToDoItemsClient : IToDoItemsClient
{
    private readonly HttpClient httpClient;
    public ToDoItemsClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ToDoItemView> CreateItemAsync(ToDoItemView item)
    {
        var request = new ToDoItemCreateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
        var response = await httpClient.PostAsJsonAsync("api/ToDoItems", request);
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<ToDoItemGetResponseDto>();

        return new ToDoItemView
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            Category = dto.Category
        };
    }
    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();

        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");
            toDoItemViews = response.Select(dto => new ToDoItemView()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Category = dto.Category
            }).ToList();

            return toDoItemViews;
        }
        catch
        {
            return toDoItemViews;
        }

    }

    public async Task<ToDoItemView> ReadItemByIdAsync(int itemId)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

        var toDoItem = new ToDoItemView()
        {
            Id = response.Id,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted,
            Category = response.Category
        };


        return toDoItem;
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
        await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
    }

    public async Task DeleteItemAsync(int itemId) => await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");
}


