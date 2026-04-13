using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoSK.Api.Models;

public class ChatSession
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)] public string SessionId { get; set; } = string.Empty;
    [MaxLength(200)] public string? Titulo { get; set; }
    [MaxLength(100)] public string? UsuarioId { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime UltimaActividad { get; set; } = DateTime.UtcNow;
    public bool Activa { get; set; } = true;
    public List<ChatMessage> Mensajes { get; set; } = new();
}

public class ChatMessage
{
    [Key] public int Id { get; set; }
    public int ChatSessionId { get; set; }
    [Required, MaxLength(20)] public string Rol { get; set; } = "user";
    [Required] public string Contenido { get; set; } = string.Empty;
    [MaxLength(50)] public string? TipoContenido { get; set; } = "text";
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    [ForeignKey(nameof(ChatSessionId))] public ChatSession? Session { get; set; }
}

public class PluginInvocationLog
{
    [Key] public int Id { get; set; }
    [MaxLength(100)] public string PluginName { get; set; } = string.Empty;
    [MaxLength(100)] public string FunctionName { get; set; } = string.Empty;
    public string? Argumentos { get; set; }
    public string? Resultado { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public int DuracionMs { get; set; }
}

public class ContenidoGenerado
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string Tipo { get; set; } = string.Empty;
    [MaxLength(300)] public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public string? AudioUrl { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
