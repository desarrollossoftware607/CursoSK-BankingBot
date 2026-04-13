using System.ComponentModel.DataAnnotations;

namespace CursoSK.BankingBot.Models;

/// <summary>Ley o normativa legal almacenada para consulta del asistente legal.</summary>
public class LeyNormativa
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? NumeroLey { get; set; } // Ej: "Decreto 170-2016"

    [MaxLength(50)]
    public string Categoria { get; set; } = string.Empty; // "bancaria", "proteccion_consumidor", "lavado_activos", "tributaria"

    [Required]
    public string Contenido { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? FuenteUrl { get; set; }

    public DateTime FechaPublicacion { get; set; }
    public bool Vigente { get; set; } = true;
}

/// <summary>Artículo específico de una ley (para búsqueda granular).</summary>
public class ArticuloLey
{
    [Key]
    public int Id { get; set; }

    public int LeyNormativaId { get; set; }

    [MaxLength(20)]
    public string NumeroArticulo { get; set; } = string.Empty;

    [Required]
    public string Texto { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Resumen { get; set; }

    public LeyNormativa? Ley { get; set; }
}
