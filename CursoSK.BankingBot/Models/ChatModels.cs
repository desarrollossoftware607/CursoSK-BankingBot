using System.ComponentModel.DataAnnotations;

namespace CursoSK.BankingBot.Models;

/// <summary>Sesión de conversación con el asistente bancario.</summary>
public class ChatSession
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string SessionId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Titulo { get; set; }

    [MaxLength(100)]
    public string? UsuarioId { get; set; }

    [MaxLength(30)]
    public string Canal { get; set; } = "web"; // "web", "whatsapp", "api"

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime UltimaActividad { get; set; } = DateTime.UtcNow;
    public bool Activa { get; set; } = true;

    public List<ChatMessage> Mensajes { get; set; } = new();
}

/// <summary>Mensaje individual en una sesión de chat.</summary>
public class ChatMessage
{
    [Key]
    public int Id { get; set; }

    public int ChatSessionId { get; set; }

    [Required, MaxLength(20)]
    public string Rol { get; set; } = "user"; // "system", "user", "assistant"

    [Required]
    public string Contenido { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? TipoContenido { get; set; } = "text";

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public ChatSession? Session { get; set; }
}

/// <summary>Registro de invocaciones de plugins (auditoría).</summary>
public class PluginLog
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string PluginName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string FunctionName { get; set; } = string.Empty;

    public string? Argumentos { get; set; }
    public string? Resultado { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int DuracionMs { get; set; }
}
