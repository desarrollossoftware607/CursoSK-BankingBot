# Paso a Paso — Sesión 4: Chat Multimodal + Persistencia EF Core

> **Rama Git:** `sesion/04`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Models/ChatModels.cs` | **Crear** |
| `Data/AppDbContext.cs` | **Crear** |
| `Controllers/ChatController.cs` | Modificar (agregar endpoint imagen) |
| `Services/ChatSessionService.cs` | Modificar (agregar método imagen) |
| `DTOs/Requests.cs` | Modificar (agregar DTO) |
| `Program.cs` | Modificar (registrar EF Core) |

---

## 1. Instalar paquetes NuGet adicionales

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.4
```

---

## 2. Models/ChatModels.cs

```csharp
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
```

---

## 3. Data/AppDbContext.cs

```csharp
using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Models;

namespace CursoSK.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<PluginInvocationLog> PluginLogs => Set<PluginInvocationLog>();
    public DbSet<ContenidoGenerado> ContenidosGenerados => Set<ContenidoGenerado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatSession>().HasIndex(s => s.SessionId).IsUnique();

        // Seed data
        modelBuilder.Entity<ChatSession>().HasData(
            new ChatSession { Id = 1, SessionId = "demo-session",
                Titulo = "Sesión de demostración", UsuarioId = "demo",
                FechaCreacion = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 1, 8, 30, 0, DateTimeKind.Utc) });

        modelBuilder.Entity<ChatMessage>().HasData(
            new ChatMessage { Id = 1, ChatSessionId = 1, Rol = "system",
                Contenido = "Eres un asistente del curso de Semantic Kernel.",
                Fecha = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 2, ChatSessionId = 1, Rol = "user",
                Contenido = "¿Qué es Semantic Kernel?",
                Fecha = new DateTime(2026, 4, 1, 8, 1, 0, DateTimeKind.Utc) },
            new ChatMessage { Id = 3, ChatSessionId = 1, Rol = "assistant",
                Contenido = "Semantic Kernel es un SDK open-source de Microsoft para integrar LLMs en aplicaciones.",
                Fecha = new DateTime(2026, 4, 1, 8, 1, 30, DateTimeKind.Utc) });
    }
}
```

---

## 4. Agregar endpoint de imagen al ChatController

Agregar este método al `ChatController` existente:

```csharp
[HttpPost("{sessionId}/imagen")]
public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
{
    var respuesta = await _chatService.EnviarMensajeConImagen(
        sessionId, request.Pregunta ?? "Describe esta imagen en detalle", request.ImagenUrl);
    return Ok(new { sessionId, respuesta });
}
```

---

## 5. Agregar método al ChatSessionService

Agregar este método al `ChatSessionService` existente:

```csharp
public async Task<string> EnviarMensajeConImagen(string sessionId, string pregunta, string imagenUrl)
{
    var history = _sessions.GetOrAdd(sessionId, _ =>
        new ChatHistory("Eres un asistente multimodal capaz de analizar imágenes."));

    var contents = new ChatMessageContentItemCollection();
    contents.Add(new TextContent(pregunta));
    contents.Add(new ImageContent(new Uri(imagenUrl)));
    history.AddUserMessage(contents);

    var chatService = _kernel.GetRequiredService<IChatCompletionService>();
    var response = await chatService.GetChatMessageContentAsync(history);
    history.AddAssistantMessage(response.Content!);
    return response.Content!;
}
```

---

## 6. Agregar DTO a DTOs/Requests.cs

```csharp
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);
```

---

## 7. Registrar EF Core en Program.cs

Agregar antes de `builder.Build()`:

```csharp
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));
```

Agregar después de `builder.Build()`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
```

No olvidar el `using` al inicio de Program.cs:

```csharp
using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Data;
```

---

## 8. Probar

```powershell
dotnet run
```

- `POST /api/chat/session1/imagen` → `{ "imagenUrl": "https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png", "pregunta": "¿Qué ves en esta imagen?" }`
