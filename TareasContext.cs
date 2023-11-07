using Microsoft.EntityFrameworkCore;
using projectef.Models;

namespace projectef;

public class TareasContext : DbContext
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public TareasContext(DbContextOptions<TareasContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        List<Categoria> categoriasInit = new List<Categoria>();

        categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("f457ac97-a506-4fce-a6e7-c0a44b733bb0"), Nombre = "Casa", Descripcion = "Tareas de casa", Peso = 10 });

        categoriasInit.Add(new Categoria { CategoriaId = Guid.Parse("67c8f583-5f5d-49ff-94f1-b459a5c77cc9"), Nombre = "Trabajo", Descripcion = "Tareas de trabajo", Peso = 30 });

        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p => p.CategoriaId);
            categoria.Property(p => p.Nombre).HasMaxLength(150).IsRequired(false);
            categoria.Property(p => p.Descripcion).HasMaxLength(500);
            categoria.Property(p => p.Peso);

            categoria.HasData(categoriasInit);
        });

        // Lo mismo que se hizo con categoria, con tareas, data semilla

        List<Tarea> tareasInit = new List<Tarea>();

        tareasInit.Add(new Tarea {
            TareaId = Guid.Parse("65ef83f3-fa49-4ed3-8b0a-0669fbbec3da"),
            CategoriaId = Guid.Parse("f457ac97-a506-4fce-a6e7-c0a44b733bb0"),
            PrioridadTarea = Prioridad.Media,
            Titulo = "Limpiar cocina",
            Descripcion = "Limpiar cocina lo mas pronto posible",
            FechaCreacion = DateTime.Now
        });

        tareasInit.Add(new Tarea {
            TareaId = Guid.Parse("e7acc17a-8162-4c9b-a494-e2bb217e2a7e"),
            CategoriaId = Guid.Parse("67c8f583-5f5d-49ff-94f1-b459a5c77cc9"),
            PrioridadTarea = Prioridad.Baja,
            Titulo = "Editar informe",
            Descripcion = "Editar y revisar informe de ventas",
            FechaCreacion = DateTime.Now
        });

        modelBuilder.Entity<Tarea>(tarea =>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(p => p.TareaId);
            tarea.Property(p => p.Titulo).HasMaxLength(150).IsRequired();
            tarea.Property(p => p.Descripcion).HasMaxLength(500).IsRequired(false);
            tarea.Property(p => p.FechaCreacion).HasColumnType("date");
            tarea.Property(p => p.PrioridadTarea);
            tarea.Ignore(p => p.Resumen);
            tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);

            tarea.HasData(tareasInit);
        });
    }
}