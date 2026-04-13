# Paso a Paso — Sesión 3: Blog Generator + Chat History

> **Rama Git:** `sesion/03`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Services/BlogService.cs` | **Crear** |
| `Controllers/BlogController.cs` | **Crear** |
| `Services/ChatSessionService.cs` | **Crear** |
| `Controllers/ChatController.cs` | **Crear** |
| `DTOs/Requests.cs` | Modificar (agregar DTOs) |
| `Program.cs` | Modificar (registrar servicios) |

---

## 1. Services/BlogService.cs

```csharp
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Services;

public class BlogService
{
    private readonly Kernel _kernel;
    public BlogService(Kernel kernel) => _kernel = kernel;

    public async Task<string> GenerarContenidoBlog(string tema)
    {
        var blogPrompt = $"""
            Genera una publicación de blog detallada acerca de {tema}.
            Debe incluir introducción, varios párrafos, code snippets si es necesario, y conclusión.
            Separa cada sección con un encabezado.
            Es OBLIGATORIO usar los siguientes bloques Gutenberg:

            Para encabezado: <!-- wp:heading --><h2 class="wp-block-heading">TEXTO</h2><!-- /wp:heading -->
            Para párrafo: <!-- wp:paragraph --><p>TEXTO</p><!-- /wp:paragraph -->
            Para lista: <!-- wp:list --><ul class="wp-block-list"><li>ITEM</li></ul><!-- /wp:list -->
            Para código: <!-- wp:code --><pre class="wp-block-code"><code>CÓDIGO</code></pre><!-- /wp:code -->
            """;
        var result = await _kernel.InvokePromptAsync(blogPrompt);
        return result.ToString();
    }
}
```

---

## 2. Controllers/BlogController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("3️⃣ Blog — Sesión 3")]
public class BlogController : ControllerBase
{
    private readonly BlogService _blogService;
    public BlogController(BlogService blogService) => _blogService = blogService;

    [HttpPost("generar")]
    public async Task<IActionResult> GenerarBlogPost([FromBody] BlogRequest request)
    {
        var contenido = await _blogService.GenerarContenidoBlog(request.Tema);
        return Ok(new { tema = request.Tema, contenidoHtml = contenido });
    }
}
```

---

## 3. Services/ChatSessionService.cs

```csharp
using System.Collections.Concurrent;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CursoSK.Api.Services;

public class ChatSessionService
{
    private readonly ConcurrentDictionary<string, ChatHistory> _sessions = new();
    private readonly Kernel _kernel;
    public ChatSessionService(Kernel kernel) => _kernel = kernel;

    public async Task<string> EnviarMensaje(string sessionId, string mensaje)
    {
        var history = _sessions.GetOrAdd(sessionId, _ =>
            new ChatHistory("Eres un asistente útil que recuerda toda la conversación."));
        history.AddUserMessage(mensaje);
        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(history);
        history.AddAssistantMessage(response.Content!);
        return response.Content!;
    }

    public ChatHistory? ObtenerHistorial(string sessionId) =>
        _sessions.GetValueOrDefault(sessionId);

    public bool EliminarSesion(string sessionId) =>
        _sessions.TryRemove(sessionId, out _);

    public IEnumerable<string> ListarSesiones() => _sessions.Keys;
}
```

---

## 4. Controllers/ChatController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("4️⃣ Chat — Sesiones 3-4")]
public class ChatController : ControllerBase
{
    private readonly ChatSessionService _chatService;
    public ChatController(ChatSessionService chatService) => _chatService = chatService;

    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
    {
        var respuesta = await _chatService.EnviarMensaje(sessionId, request.Mensaje);
        return Ok(new { sessionId, respuesta });
    }

    [HttpGet("{sessionId}/historial")]
    public IActionResult ObtenerHistorial(string sessionId)
    {
        var history = _chatService.ObtenerHistorial(sessionId);
        if (history == null) return NotFound("Sesión no encontrada");
        return Ok(history.Select(m => new { rol = m.Role.Label, contenido = m.Content }));
    }

    [HttpGet("sesiones")]
    public IActionResult ListarSesiones() => Ok(_chatService.ListarSesiones());

    [HttpDelete("{sessionId}")]
    public IActionResult EliminarSesion(string sessionId)
    {
        _chatService.EliminarSesion(sessionId);
        return Ok(new { mensaje = "Sesión eliminada" });
    }
}
```

---

## 5. Agregar DTOs a DTOs/Requests.cs

Agregar al final del archivo:

```csharp
public record BlogRequest(string Tema);
public record ChatMensajeRequest(string Mensaje);
```

---

## 6. Registrar servicios en Program.cs

Agregar antes de `builder.Build()`:

```csharp
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<ChatSessionService>();
```

---

## 7. Probar

```powershell
dotnet run
```

- `POST /api/blog/generar` → `{ "tema": "Semantic Kernel en .NET" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "Mi nombre es José" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }` → ¡Recuerda!
- `GET /api/chat/session1/historial`
