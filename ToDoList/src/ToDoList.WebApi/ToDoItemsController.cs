namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //localhost::5000/api/ToDoItems
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> items = [];

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //použijeme DTO = Data Transfer Object
    {
        var item = request.ToDomain();

        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item);
        }
        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(nameof(ReadById), new { item.ToDoItemId }, item);
    }

    [HttpGet]
    public IActionResult Read()
    {
        try
        {
            if (items.Count == 0)
            {
                return NotFound();
            }

            var dtoList = items.Select(ToDoItemGetResponseDto.FromDomain).ToList();
            return Ok(dtoList);
        }
        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public IActionResult ReadById(int toDoItemId) //api/ToDoItems<id>
    {
        try
        {
            throw new Exception("Něco se opravdu nepovedlo.");
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);  //500
        }

        return Ok();
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok();
    }
}
