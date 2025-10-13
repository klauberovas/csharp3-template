namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> Items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            item.ToDoItemId = Items.Count == 0 ? 1 : Items.Max(o => o.ToDoItemId) + 1;
            Items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(ReadById), new { item.ToDoItemId }, item);
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            if (Items.Count == 0)
            {
                return NotFound();
            }

            var dtoList = Items.Select(ToDoItemGetResponseDto.FromDomain).ToList();
            return Ok(dtoList);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) //api/ToDoItems<id>
    {
        try
        {
            var item = Items.Find(i => i.ToDoItemId == toDoItemId);

            if (item == null)
            {
                return NotFound();
            }

            var dto = ToDoItemGetResponseDto.FromDomain(item);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);  //500
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        try
        {
            int index = Items.FindIndex(i => i.ToDoItemId == toDoItemId);

            if (index == -1)
            {
                return NotFound();
            }

            Items[index] = request.ToDomain(Items[index]);

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        try
        {
            int index = Items.FindIndex(i => i.ToDoItemId == toDoItemId);

            if (index == -1)
            {
                return NotFound();
            }

            Items.RemoveAt(index);

            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }
}
