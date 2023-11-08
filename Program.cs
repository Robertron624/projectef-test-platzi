using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;
using projectef.Models;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));

builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) => {
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos creada: " + dbContext.Database.IsInMemory());
});

app.MapGet("/api/categorias", async ([FromServices] TareasContext dbContext) => {
    var categorias = await dbContext.Categorias.ToListAsync();
    return Results.Ok(categorias);
});

app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) => {
    var tareas = await dbContext.Tareas.Include(p => p.Categoria).ToListAsync();
    return Results.Ok(tareas);
});


// Obtener tarea por id

app.MapGet("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, Guid id) => {
    var tarea = await dbContext.Tareas.FindAsync(id);
    if (tarea == null) return Results.NotFound();
    return Results.Ok(tarea);
});

// Añadir tarea

app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) => {
    
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;

    await dbContext.Tareas.AddAsync(tarea);
    await dbContext.SaveChangesAsync();

    return Results.Ok(tarea);

});

// Actualizar tarea

app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) => {
    
    var tareaActual = await dbContext.Tareas.FindAsync(id);

    if(tareaActual != null) {
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.Descripcion = tarea.Descripcion;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.CategoriaId = tarea.CategoriaId;

        await dbContext.SaveChangesAsync();

        return Results.Ok(tareaActual);
    }


    return Results.NotFound();
});

// Eliminar tarea

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext,[FromRoute] Guid id) => {
    
    var tareaActual = await dbContext.Tareas.FindAsync(id);

    if(tareaActual != null) {
        dbContext.Tareas.Remove(tareaActual);
        await dbContext.SaveChangesAsync();

        return Results.Ok(tareaActual);
    }

    return Results.NotFound();
});


app.Run();
