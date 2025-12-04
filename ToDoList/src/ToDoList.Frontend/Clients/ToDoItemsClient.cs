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
            var dtos = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

            toDoItemViews = dtos.Select(dto => new ToDoItemView()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Category = dto.Category
            }).ToList();

            return toDoItemViews;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Chyba při načítání úkolů: {ex.Message}");
            return toDoItemViews;
        }
    }

    public async Task<ToDoItemView> ReadItemByIdAsync(int itemId)
    {
        try
        {
            var dto = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

            return new ToDoItemView()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Category = dto.Category
            };
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            if (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new InvalidOperationException("Došlo k chybě na serveru (500). Zkuste to prosím později.");
            }

            throw;
        }
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {

        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);

        try
        {
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Úkol s ID {item.Id} nebyl nalezen.", ex);
            }


            if (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new InvalidOperationException("Došlo k chybě na serveru (500). Zkuste to později.", ex);
            }

            throw;
        }
    }

    public async Task DeleteItemAsync(int itemId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException($"Úkol s ID {itemId} nebyl nalezen.", ex);
            }

            if (ex.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new InvalidOperationException("Došlo k chybě na serveru (500). Zkuste to později.", ex);
            }

            throw;
        }
    }
}


