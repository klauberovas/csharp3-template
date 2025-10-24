namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{
    public void ApplyToDomain(ToDoItem item)
    {
        item.Name = Name;
        item.Description = Description;
        item.IsCompleted = IsCompleted;
    }
}
