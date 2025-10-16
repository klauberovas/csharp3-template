namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;

public static class ActionResultExtensions
{
    public static T? GetValue<T>(this ActionResult<T> response) => response.Result is null
        ? response.Value
        : (T?)(response.Result as ObjectResult)?.Value;
}
