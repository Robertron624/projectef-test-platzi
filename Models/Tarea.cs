using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectef.Models;

public class Tarea
{
    // [Key]
    public Guid TareaId { get; set; }

    // [Required]
    // [ForeignKey("CategoriaId")]
    public Guid CategoriaId { get; set; }

    // [Required]
    // [MaxLength(200)]
    public string Titulo { get; set; }

    // [MaxLength(500)]
    public string Descripcion { get; set; }
    public Prioridad PrioridadTarea { get; set; }
    public DateTime FechaCreacion { get; set; }
    public virtual Categoria Categoria { get; set; }

    //[NotMapped] // Solo para guardar en memoria, no en la base de datos
    public string Resumen {get; set;}
}

public enum Prioridad
{
    Baja,
    Media,
    Alta
}