namespace ToDoList.Domain.DTOs;

public record class ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted)
{

}
