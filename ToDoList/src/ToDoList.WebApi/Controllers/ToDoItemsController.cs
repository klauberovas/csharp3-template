namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> Items = []; // smazat po úkolu
    private readonly ToDoItemsContext context;

    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return CreatedAtAction(
            nameof(ReadById),
            new { toDoItemId = item.ToDoItemId },
            ToDoItemGetResponseDto.FromDomain(item));
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
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
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
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
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
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

    public void AddItemToStorage(ToDoItem item)
    {
        Items.Add(item);
    }
}
