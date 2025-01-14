using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using ToDoApp.Data.Contexts;
using ToDoApp.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<ToDoContext>("postgresdb");

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var todoContext = scope.ServiceProvider.GetRequiredService<ToDoContext>();
    todoContext.Database.EnsureCreated();
}

app.MapGet("/todos", ([FromServices] ToDoContext db) =>
{
    var todos = db.ToDos.ToList();

    return Results.Json(todos);
});

app.MapGet("/todos/{id}", ([FromServices] ToDoContext db, int id) =>
{
    var todo = db.ToDos.Find(id);

    if (todo == null)
    {
        return Results.NotFound();
    }

    return Results.Json(todo);
});

app.MapPost("/todos", ([FromServices] ToDoContext db, ToDoEntry todo) =>
{
    todo.IsComplete ??= false;

    if (todo.Title == null)
    {
        return Results.BadRequest();
    }
    
    db.ToDos.Add(todo);
    db.SaveChangesAsync();

    return Results.Created();
});

app.MapPatch("/todos/{id}", ([FromServices] ToDoContext db, [FromRoute] int id, [FromBody] ToDoEntry entry) =>
{
    var todo = db.ToDos.Find(id);

    if (todo == null)
    {
        return Results.StatusCode(304);
    }

    if (entry.IsComplete != null)
    {
        todo.IsComplete = entry.IsComplete;
    }

    if (!string.IsNullOrEmpty(entry.Title))
    {
        todo.Title = entry.Title;
    }
    
    db.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("/todos/{id}", ([FromServices] ToDoContext db, int id) =>
{
    var todo = db.ToDos.Find(id);

    if (todo == null)
    {
        return Results.NotFound();
    }

    db.ToDos.Remove(todo);
    db.SaveChanges();

    return Results.NoContent();
});

app.MapDefaultEndpoints();

app.Run();