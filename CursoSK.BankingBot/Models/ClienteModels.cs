using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoSK.BankingBot.Models;

/// <summary>Cliente del banco (solicitante de préstamo).</summary>
public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string Identidad { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string NombreCompleto { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Correo { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(300)]
    public string? Direccion { get; set; }

    public DateTime FechaNacimiento { get; set; }

    [MaxLength(50)]
    public string? Ocupacion { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal IngresoMensual { get; set; }

    [MaxLength(20)]
    public string Estado { get; set; } = "activo"; // activo, inactivo, bloqueado

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navegación
    public List<SolicitudPrestamo> Solicitudes { get; set; } = new();
    public List<DocumentoCliente> Documentos { get; set; } = new();
}

/// <summary>Documento digitalizado del cliente (DPI, constancia laboral, etc.).</summary>
public class DocumentoCliente
{
    [Key]
    public int Id { get; set; }

    public int ClienteId { get; set; }

    [Required, MaxLength(50)]
    public string TipoDocumento { get; set; } = string.Empty; // "dpi", "constancia_laboral", "recibo_servicios", "estado_cuenta"

    [MaxLength(500)]
    public string? RutaArchivo { get; set; }

    [MaxLength(20)]
    public string Estado { get; set; } = "pendiente"; // pendiente, validado, rechazado

    public string? TextoExtraido { get; set; } // OCR / Document Intelligence

    public string? ResultadoAnalisis { get; set; } // JSON con el análisis de IA

    public DateTime FechaCarga { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ClienteId))]
    public Cliente? Cliente { get; set; }
}
