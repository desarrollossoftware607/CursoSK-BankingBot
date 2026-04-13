using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoSK.BankingBot.Models;

/// <summary>Solicitud de préstamo bancario.</summary>
public class SolicitudPrestamo
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string NumeroSolicitud { get; set; } = string.Empty;

    public int ClienteId { get; set; }

    [Required, MaxLength(50)]
    public string TipoPrestamo { get; set; } = string.Empty; // "personal", "hipotecario", "vehicular", "empresarial"

    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoSolicitado { get; set; }

    public int PlazoMeses { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TasaInteres { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CuotaMensualEstimada { get; set; }

    [Required, MaxLength(30)]
    public string Estado { get; set; } = "borrador";
    // borrador → en_revision → documentos_pendientes → pre_aprobado → aprobado → desembolsado
    // borrador → en_revision → rechazado

    [MaxLength(500)]
    public string? MotivoRechazo { get; set; }

    [MaxLength(1000)]
    public string? ObservacionesLegales { get; set; }

    public int PuntajeRiesgo { get; set; } // 0-100 calculado por IA

    [MaxLength(20)]
    public string? NivelRiesgo { get; set; } // "bajo", "medio", "alto", "muy_alto"

    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
    public DateTime? FechaAprobacion { get; set; }
    public DateTime? FechaDesembolso { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public Cliente? Cliente { get; set; }

    public List<RequisitoSolicitud> Requisitos { get; set; } = new();
    public List<HistorialEstado> HistorialEstados { get; set; } = new();
}

/// <summary>Requisito individual de una solicitud de préstamo.</summary>
public class RequisitoSolicitud
{
    [Key]
    public int Id { get; set; }

    public int SolicitudPrestamoId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Descripcion { get; set; }

    public bool Obligatorio { get; set; } = true;

    public bool Cumplido { get; set; }

    [MaxLength(500)]
    public string? Observacion { get; set; }

    public DateTime? FechaCumplimiento { get; set; }

    [ForeignKey(nameof(SolicitudPrestamoId))]
    public SolicitudPrestamo? Solicitud { get; set; }
}

/// <summary>Historial de cambios de estado de la solicitud.</summary>
public class HistorialEstado
{
    [Key]
    public int Id { get; set; }

    public int SolicitudPrestamoId { get; set; }

    [Required, MaxLength(30)]
    public string EstadoAnterior { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string EstadoNuevo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Observacion { get; set; }

    [MaxLength(100)]
    public string? Usuario { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(SolicitudPrestamoId))]
    public SolicitudPrestamo? Solicitud { get; set; }
}
