using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI Container
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{

    // apply pending migrations on startup
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ToDoItemsContext>();
        db.Database.Migrate();
    }
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("v1/swagger.json", "ToDoList API V1"));
}

app.Run();

