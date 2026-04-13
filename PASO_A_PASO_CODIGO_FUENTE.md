# Guía Paso a Paso — Código Fuente de Ambos Proyectos

## CursoSK.Api + CursoSK.BankingBot — Organizado por Sesión

> **Alineado con:** `CURSO_UNIFICADO_JORNALIZACION.md` — 10 sesiones / 30 horas

Esta guía es para ir **copiando y pegando código** sesión por sesión. Al final tendrás dos APIs completas con ~30 endpoints en total.

| Proyecto | Puerto | Descripción |
|---|---|---|
| **CursoSK.Api** | 5192 | API genérica de IA (Blog, Chat, Plugins, Prompting, RAG) |
| **CursoSK.BankingBot** | 5290 | Bot bancario (Onboarding, Legal, Audio, RAG leyes) |

---

## PASO 0 — Crear Ambos Proyectos y Estructura Inicial

### 0.1 Crear los proyectos desde terminal

```bash
# Proyecto 1: API genérica
dotnet new webapi -n CursoSK.Api --use-controllers
cd CursoSK.Api

# Instalar paquetes
dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.4
dotnet add package Microsoft.SemanticKernel.PromptTemplates.Handlebars --version 1.48.0
dotnet add package Microsoft.SemanticKernel.Yaml --version 1.48.0
dotnet add package Microsoft.SemanticKernel.Plugins.Core --prerelease
dotnet add package System.Numerics.Tensors --version 9.0.1

cd ..

# Proyecto 2: Bot bancario
dotnet new webapi -n CursoSK.BankingBot --use-controllers
cd CursoSK.BankingBot

dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.4
dotnet add package System.Numerics.Tensors --version 9.0.1
```

### 0.2 Estructura de carpetas (ambos proyectos)

```powershell
# Ejecutar dentro de cada proyecto
"Controllers","Services","Plugins","Filters","Models","Data","DTOs","Prompts" | ForEach-Object { New-Item -ItemType Directory -Name $_ -Force }
```

### 0.3 Eliminar archivos de plantilla

Eliminar los archivos generados por `dotnet new webapi` que no necesitamos:
- `WeatherForecast.cs`
- `Controllers/WeatherForecastController.cs`

---

## SESIÓN 1 — Fundamentos SK + Setup Azure

### CursoSK.Api

#### 1.1 — appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=cursosk.db"
  },
  "LLMSettings": {
    "Provider": "azure",
    "OpenAI": {
      "ModelId": "gpt-4o-mini",
      "ApiKey": "sk-TU-API-KEY-AQUI"
    },
    "AzureOpenAI": {
      "DeploymentName": "gpt-35-turbo-16k",
      "Endpoint": "https://tu-recurso.openai.azure.com/",
      "ApiKey": "TU-API-KEY-AQUI"
    },
    "Embedding": {
      "DeploymentName": "text-embedding-3-small",
      "Endpoint": "https://tu-recurso.openai.azure.com/",
      "ApiKey": "TU-API-KEY-AQUI"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### 1.2 — Program.cs (versión inicial — Sesión 1)

```csharp
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CursoSK.Api", Version = "v1" });
});

// Configurar Semantic Kernel
var llmProvider = builder.Configuration["LLMSettings:Provider"]?.ToLower() ?? "azure";
var kernelBuilder = Kernel.CreateBuilder();

if (llmProvider == "openai")
{
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: builder.Configuration["LLMSettings:OpenAI:ModelId"]!,
        apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!);
}
else
{
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["LLMSettings:AzureOpenAI:DeploymentName"]!,
        endpoint: builder.Configuration["LLMSettings:AzureOpenAI:Endpoint"]!,
        apiKey: builder.Configuration["LLMSettings:AzureOpenAI:ApiKey"]!);
}

builder.Services.AddSingleton(kernelBuilder.Build());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
```

#### 1.3 — DTOs/Requests.cs (versión inicial)

```csharp
namespace CursoSK.Api.DTOs;

public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
```

#### 1.4 — Controllers/KernelController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("1️⃣ Kernel — Sesión 1")]
public class KernelController : ControllerBase
{
    private readonly Kernel _kernel;
    public KernelController(Kernel kernel) => _kernel = kernel;

    [HttpPost("prompt")]
    public async Task<IActionResult> InvokePrompt([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { response = result.ToString() });
    }

    [HttpPost("prompt/configurado")]
    public async Task<IActionResult> InvokePromptConSettings([FromBody] PromptConSettingsRequest request)
    {
        var skSettings = new OpenAIPromptExecutionSettings
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(skSettings));
        return Ok(new { response = result.ToString() });
    }
}
```

**Probar:** `dotnet run` → http://localhost:5192/swagger → `POST /api/kernel/prompt`

### CursoSK.BankingBot

#### 1.5 — Program.cs (BankingBot — versión inicial)

```csharp
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CursoSK.BankingBot", Version = "v1" });
});

var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddAzureOpenAIChatCompletion(
    deploymentName: builder.Configuration["AzureOpenAI:DeploymentName"]!,
    endpoint: builder.Configuration["AzureOpenAI:Endpoint"]!,
    apiKey: builder.Configuration["AzureOpenAI:ApiKey"]!);

builder.Services.AddSingleton(kernelBuilder.Build());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
```

### Azure Setup

Ejecutar: `Scripts/Azure/01-crear-recurso-openai.ps1`

---

## SESIÓN 2 — Servicios Multimodales

### CursoSK.Api

#### 2.1 — Controllers/MultimodalController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("2️⃣ Multimodal — Sesión 2")]
public class MultimodalController : ControllerBase
{
    private readonly Kernel _kernel;
    public MultimodalController(Kernel kernel) => _kernel = kernel;

    [HttpPost("stream")]
    public async Task StreamChat([FromBody] PromptRequest request)
    {
        Response.ContentType = "text/event-stream";
        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        await foreach (var chunk in chatService.GetStreamingChatMessageContentsAsync(request.Prompt))
        {
            await Response.WriteAsync($"data: {chunk.Content}\n\n");
            await Response.Body.FlushAsync();
        }
    }
}
```

**Probar:** `POST /api/multimodal/stream` → streaming token por token

### CursoSK.BankingBot

#### 2.2 — Controllers/AudioController.cs (BankingBot)

Agregar endpoint de transcripción de audio con Whisper para transcribir llamadas de servicio al cliente.

### Azure Setup

Ejecutar: `Scripts/Azure/02-crear-deployment-whisper.ps1`

---

## SESIÓN 3 — Blog Generator + Chat History

### CursoSK.Api

#### 3.1 — Services/BlogService.cs

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

#### 3.2 — Controllers/BlogController.cs

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

#### 3.3 — Services/ChatSessionService.cs

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

#### 3.4 — Controllers/ChatController.cs

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

#### 3.5 — Agregar DTOs (en DTOs/Requests.cs)

```csharp
// Agregar a DTOs/Requests.cs:
public record BlogRequest(string Tema);
public record ChatMensajeRequest(string Mensaje);
```

#### 3.6 — Registrar servicios en Program.cs

```csharp
// Agregar antes de builder.Build():
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<ChatSessionService>();
```

**Probar:**
- `POST /api/blog/generar` → `{ "tema": "Semantic Kernel en .NET" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "Mi nombre es José" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }` → ¡Recuerda!
- `GET /api/chat/session1/historial`

---

## SESIÓN 4 — Chat Multimodal + EF Core

### CursoSK.Api

#### 4.1 — Models/ChatModels.cs

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

#### 4.2 — Data/AppDbContext.cs

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

#### 4.3 — Agregar endpoint de imagen al ChatController

```csharp
// Agregar al ChatController existente:
[HttpPost("{sessionId}/imagen")]
public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
{
    var respuesta = await _chatService.EnviarMensajeConImagen(
        sessionId, request.Pregunta ?? "Describe esta imagen en detalle", request.ImagenUrl);
    return Ok(new { sessionId, respuesta });
}
```

#### 4.4 — Agregar método EnviarMensajeConImagen al ChatSessionService

```csharp
// Agregar al ChatSessionService existente:
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

#### 4.5 — Agregar DTO y registrar EF Core en Program.cs

```csharp
// DTOs/Requests.cs — agregar:
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);

// Program.cs — agregar antes de Build():
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));

// Después de Build():
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}
```

---

## SESIÓN 5 — Plugins y Function Calling

### CursoSK.Api

#### 5.1 — Plugins/ClimaPlugin.cs

```csharp
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class ClimaPlugin
{
    private static readonly Dictionary<string, string> _climas = new(StringComparer.OrdinalIgnoreCase)
    {
        ["tegucigalpa"] = "☀️ 28°C, Soleado",
        ["san pedro sula"] = "🌤️ 32°C, Parcialmente nublado",
        ["la ceiba"] = "🌧️ 29°C, Lluvioso",
        ["roatan"] = "☀️ 31°C, Soleado con brisa",
        ["comayagua"] = "⛅ 27°C, Nublado",
        ["choluteca"] = "🔥 36°C, Muy caluroso"
    };

    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad específica")]
    public string ObtenerClima([Description("Nombre de la ciudad")] string ciudad) =>
        _climas.GetValueOrDefault(ciudad, $"No tengo datos del clima para {ciudad}");

    [KernelFunction("obtener_fecha_hora")]
    [Description("Obtiene la fecha y hora actual del servidor")]
    public string ObtenerFechaHora() =>
        $"Fecha: {DateTime.Now:dd/MM/yyyy}, Hora: {DateTime.Now:hh:mm tt}";
}
```

#### 5.2 — Plugins/MathPlugin.cs

```csharp
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class MathPlugin
{
    [KernelFunction("sumar")]
    [Description("Suma dos números")]
    public double Sumar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a + b;

    [KernelFunction("restar")]
    [Description("Resta dos números (a - b)")]
    public double Restar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a - b;

    [KernelFunction("multiplicar")]
    [Description("Multiplica dos números")]
    public double Multiplicar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a * b;

    [KernelFunction("dividir")]
    [Description("Divide dos números (a / b)")]
    public string Dividir([Description("Dividendo")] double a, [Description("Divisor")] double b) =>
        b == 0 ? "Error: no se puede dividir entre cero" : (a / b).ToString("F4");
}
```

#### 5.3 — Controllers/AgentController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("5️⃣ Agent — Sesiones 5-6")]
public class AgentController : ControllerBase
{
    private readonly Kernel _kernel;
    public AgentController(Kernel kernel) => _kernel = kernel;

    [HttpPost("consultar")]
    public async Task<IActionResult> Consultar([FromBody] PromptRequest request)
    {
        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(settings));
        return Ok(new { respuesta = result.ToString() });
    }

    [HttpGet("plugins")]
    public IActionResult ListarPlugins()
    {
        var plugins = _kernel.Plugins.Select(p => new {
            plugin = p.Name,
            funciones = p.Select(f => new { nombre = f.Name, descripcion = f.Description })
        });
        return Ok(plugins);
    }
}
```

#### 5.4 — Registrar plugins en Program.cs

```csharp
// Agregar ANTES de kernelBuilder.Build():
kernelBuilder.Plugins.AddFromObject(new ClimaPlugin(), "Clima");
kernelBuilder.Plugins.AddFromType<MathPlugin>("Matematica");
```

**Probar Function Calling:**
- `POST /api/agent/consultar` → `{ "prompt": "¿Cuál es el clima en Tegucigalpa?" }`
- `POST /api/agent/consultar` → `{ "prompt": "¿Cuánto es 15 × 7 + 3?" }`
- `GET /api/agent/plugins` → listar todas las funciones disponibles

### CursoSK.BankingBot

#### 5.5 — Plugins/OnboardingPlugin.cs (BankingBot)

Crear plugin con funciones: `verificar_identidad`, `crear_cuenta`, `consultar_productos`.

#### 5.6 — Plugins/CalculadoraFinancieraPlugin.cs (BankingBot)

Crear plugin con funciones: `calcular_cuota_mensual`, `calcular_interes_total`.

#### 5.7 — Controllers/OnboardingController.cs (BankingBot)

Endpoint `POST /api/onboarding/iniciar` con Function Calling automático.

---

## SESIÓN 6 — Plugins Avanzados + Filtros

### CursoSK.Api

#### 6.1 — Filters/LoggingFilter.cs

```csharp
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Filters;

public class LoggingFilter : IFunctionInvocationFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger) => _logger = logger;

    public async Task OnFunctionInvocationAsync(
        FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        var pluginName = context.Function.PluginName;
        var functionName = context.Function.Name;
        var args = string.Join(", ", context.Arguments.Select(a => $"{a.Key}={a.Value}"));

        _logger.LogInformation("▶ Invocando {Plugin}.{Function}({Args})", pluginName, functionName, args);
        var sw = System.Diagnostics.Stopwatch.StartNew();

        await next(context);

        sw.Stop();
        _logger.LogInformation("✔ {Plugin}.{Function} completado en {Ms}ms → {Result}",
            pluginName, functionName, sw.ElapsedMilliseconds, context.Result);
    }
}
```

#### 6.2 — Registrar filtro en Program.cs

```csharp
// Agregar al kernelBuilder:
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, LoggingFilter>();
```

### CursoSK.BankingBot

#### 6.3 — Plugins/LegalPlugin.cs, Filters/AuditFilter.cs, Controllers/PrestamosController.cs, Controllers/LegalController.cs

Crear plugin legal con `HttpClient` inyectado, filtro de auditoría, y controladores de préstamos y consultas legales.

---

## SESIÓN 7 — Prompting Avanzado + Templates YAML

### CursoSK.Api

#### 7.1 — Controllers/PromptingController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("7️⃣ Prompting — Sesión 7")]
public class PromptingController : ControllerBase
{
    private readonly Kernel _kernel;
    public PromptingController(Kernel kernel) => _kernel = kernel;

    [HttpPost("zero-shot")]
    public async Task<IActionResult> ZeroShot([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { tecnica = "Zero-Shot", respuesta = result.ToString() });
    }

    [HttpPost("few-shot")]
    public async Task<IActionResult> FewShot([FromBody] PromptRequest request)
    {
        var prompt = $"""
            Clasifica el sentimiento del siguiente texto.
            Ejemplos:
            - "Me encanta este producto" → Positivo
            - "Es terrible, no sirve" → Negativo
            - "No está mal, pero podría mejorar" → Neutro

            Texto: {request.Prompt}
            Sentimiento:
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { tecnica = "Few-Shot", respuesta = result.ToString() });
    }

    [HttpPost("chain-of-thought")]
    public async Task<IActionResult> ChainOfThought([FromBody] PromptRequest request)
    {
        var prompt = $"""
            Analiza el siguiente problema paso a paso.
            Primero identifica los datos, luego aplica la lógica, y finalmente da la respuesta.

            Problema: {request.Prompt}

            Razonamiento paso a paso:
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { tecnica = "Chain-of-Thought", respuesta = result.ToString() });
    }

    [HttpPost("yaml")]
    public async Task<IActionResult> DesdeYaml([FromBody] PromptRequest request)
    {
        var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "ClasificarIntencion.yaml");
        var yaml = await System.IO.File.ReadAllTextAsync(yamlPath);
        var promptConfig = KernelFunctionYaml.ToPromptTemplateConfig(yaml);
        var factory = new HandlebarsPromptTemplateFactory();
        var function = KernelFunctionFactory.CreateFromPrompt(promptConfig, factory);
        var result = await _kernel.InvokeAsync(function, new() { ["consulta"] = request.Prompt });
        return Ok(new { tecnica = "YAML Template", respuesta = result.ToString() });
    }
}
```

#### 7.2 — Prompts/ClasificarIntencion.yaml

```yaml
name: ClasificarIntencion
description: Clasifica la intención del usuario en una categoría predefinida.
template_format: handlebars
template: |
  Eres un clasificador de intenciones para un sistema bancario.
  Analiza la consulta del usuario y clasifícala en UNA de estas categorías:
  - consulta_general: preguntas generales, saludos
  - solicitud_prestamo: quiere solicitar un préstamo
  - estado_solicitud: pregunta por el estado de una solicitud
  - consulta_legal: pregunta sobre regulaciones o normativas
  - simulacion: quiere simular un préstamo o calcular cuotas
  - reclamo: tiene una queja o problema

  Responde SOLO con el nombre de la categoría, sin explicación.
  Consulta del usuario: {{consulta}}
  Categoría:
input_variables:
  - name: consulta
    description: La consulta del usuario a clasificar
    is_required: true
execution_settings:
  default:
    max_tokens: 20
    temperature: 0.0
```

> **IMPORTANTE:** Agregar al `.csproj` para que los archivos YAML se copien al output:
> ```xml
> <ItemGroup>
>   <None Update="Prompts\**\*.yaml">
>     <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
>   </None>
> </ItemGroup>
> ```

### Azure Setup

Ejecutar: `Scripts/Azure/07-crear-deployment-embedding.ps1`

---

## SESIÓN 8 — Intro Embeddings + VectorStoreService

### CursoSK.Api

#### 8.1 — Models/DocumentoVectorial.cs

```csharp
namespace CursoSK.Api.Models;

public class DocumentoVectorial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Fuente { get; set; }
    public ReadOnlyMemory<float>? Embedding { get; set; }
}
```

#### 8.2 — Services/VectorStoreService.cs

```csharp
using System.Collections.Concurrent;
using System.Numerics.Tensors;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using CursoSK.Api.Models;

namespace CursoSK.Api.Services;

#pragma warning disable SKEXP0001
public class VectorStoreService
{
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _store = new();
    private readonly ITextEmbeddingGenerationService? _embeddingService;
    private readonly ILogger<VectorStoreService> _logger;

    public VectorStoreService(Kernel kernel, ILogger<VectorStoreService> logger)
    {
        _embeddingService = kernel.Services.GetService<ITextEmbeddingGenerationService>();
        _logger = logger;
    }

    public async Task<string> IndexarDocumento(string titulo, string contenido, string categoria, string? fuente = null)
    {
        if (_embeddingService == null)
            throw new InvalidOperationException("Embedding service no configurado.");

        var embedding = await _embeddingService.GenerateEmbeddingAsync(contenido);
        var doc = new DocumentoVectorial
        {
            Titulo = titulo, Contenido = contenido,
            Categoria = categoria, Fuente = fuente, Embedding = embedding
        };
        _store.TryAdd(doc.Id, doc);
        _logger.LogInformation("Documento indexado: {Titulo} ({Id})", titulo, doc.Id);
        return doc.Id;
    }

    public async Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        if (_embeddingService == null)
            throw new InvalidOperationException("Embedding service no configurado.");

        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(consulta);
        return _store.Values
            .Where(d => d.Embedding.HasValue)
            .Select(d => (Doc: d, Score: (double)TensorPrimitives.CosineSimilarity(
                queryEmbedding.Span, d.Embedding!.Value.Span)))
            .OrderByDescending(x => x.Score)
            .Take(top)
            .ToList();
    }

    public int Count => _store.Count;
}
#pragma warning restore SKEXP0001
```

#### 8.3 — Registrar embedding service en Program.cs

```csharp
// Agregar al kernelBuilder (si hay embedding configurado):
var embeddingDeployment = builder.Configuration["LLMSettings:Embedding:DeploymentName"];
if (!string.IsNullOrEmpty(embeddingDeployment))
{
    kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
        deploymentName: embeddingDeployment,
        endpoint: builder.Configuration["LLMSettings:Embedding:Endpoint"]!,
        apiKey: builder.Configuration["LLMSettings:Embedding:ApiKey"]!);
}

// Registrar servicio:
builder.Services.AddSingleton<VectorStoreService>();
```

---

## SESIÓN 9 — RAG Completo

### CursoSK.Api

#### 9.1 — Controllers/RAGController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("9️⃣ RAG — Sesiones 8-9")]
public class RAGController : ControllerBase
{
    private readonly VectorStoreService _vectorStore;
    private readonly Kernel _kernel;

    public RAGController(VectorStoreService vectorStore, Kernel kernel)
    {
        _vectorStore = vectorStore;
        _kernel = kernel;
    }

    [HttpPost("indexar")]
    public async Task<IActionResult> Indexar([FromBody] IndexarDocumentoRequest request)
    {
        var id = await _vectorStore.IndexarDocumento(
            request.Titulo, request.Contenido, request.Categoria, request.Fuente);
        return Ok(new { id, mensaje = "Documento indexado", totalDocumentos = _vectorStore.Count });
    }

    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);
        return Ok(resultados.Select(r => new
        {
            titulo = r.Doc.Titulo, contenido = r.Doc.Contenido,
            categoria = r.Doc.Categoria, score = Math.Round(r.Score, 4)
        }));
    }

    [HttpPost("consultar")]
    public async Task<IActionResult> ConsultarRAG([FromBody] BusquedaSemanticaRequest request)
    {
        var resultados = await _vectorStore.BuscarSimilares(request.Consulta, request.Top);
        var contexto = string.Join("\n\n---\n\n",
            resultados.Select(r => $"[{r.Doc.Titulo}] (Score: {r.Score:F3})\n{r.Doc.Contenido}"));

        var prompt = $"""
            Responde ÚNICAMENTE basándote en el siguiente contexto.
            Si no hay información suficiente, di "No tengo información suficiente."

            CONTEXTO:
            {contexto}

            PREGUNTA: {request.Consulta}
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { respuesta = result.ToString(), fragmentosUsados = resultados.Count });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedFAQs()
    {
        var faqs = new[]
        {
            ("¿Qué es Semantic Kernel?", "Semantic Kernel es un SDK open-source de Microsoft para integrar LLMs en aplicaciones.", "faq"),
            ("¿Qué es RAG?", "RAG (Retrieval-Augmented Generation) combina búsqueda vectorial con generación de texto.", "faq"),
            ("¿Qué es Function Calling?", "Function Calling permite al LLM invocar funciones C# registradas como plugins.", "faq"),
        };
        foreach (var (titulo, contenido, cat) in faqs)
            await _vectorStore.IndexarDocumento(titulo, contenido, cat, "FAQ del curso");
        return Ok(new { mensaje = $"{faqs.Length} FAQs indexadas", total = _vectorStore.Count });
    }
}
```

#### 9.2 — Agregar DTOs

```csharp
// DTOs/Requests.cs — agregar:
public record IndexarDocumentoRequest(string Titulo, string Contenido, string Categoria, string? Fuente);
public record BusquedaSemanticaRequest(string Consulta, int Top = 3);
```

**Probar RAG:**
1. `POST /api/rag/seed` → cargar FAQs de ejemplo
2. `POST /api/rag/buscar` → `{ "consulta": "¿Qué es Semantic Kernel?", "top": 3 }`
3. `POST /api/rag/consultar` → `{ "consulta": "¿Qué es RAG y para qué sirve?" }`

### CursoSK.BankingBot

#### 9.3 — RAGController (BankingBot) + LegalIndexingService

Crear RAGController con `/indexar/leyes` que lee archivos de `Docs/Leyes/*.txt`, divide en chunks y los indexa. Crear LegalController con `/consultar/rag` para consultas legales con RAG.

### Azure Setup

Ejecutar (opcional): `Scripts/Azure/09-crear-ai-search.ps1`

---

## SESIÓN 10 — Deploy a Azure + Foundry

### Deploy

Ejecutar: `Scripts/Azure/10-deploy-app-service.ps1`

El script:
1. Pregunta qué proyecto desplegar (CursoSK.Api o BankingBot)
2. Ejecuta `dotnet publish -c Release`
3. Crea zip del output
4. Crea App Service Plan + Web App en Azure
5. Configura variables de entorno (LLMSettings)
6. Ejecuta `az webapp deploy --type zip`

### Foundry

Seguir instrucciones en: `Scripts/Azure/10-foundry-setup.ps1`

### Merge a main

```bash
git checkout main
git merge sesion/10
git tag v1.0.0 -m "Curso completo - 10 sesiones"
git push origin main --tags
```

---

## Resumen de Archivos por Sesión

| Sesión | Archivos CursoSK.Api | Archivos CursoSK.BankingBot |
|---|---|---|
| **1** | Program.cs, KernelController.cs, Requests.cs, appsettings.json | Program.cs, appsettings.json |
| **2** | MultimodalController.cs | AudioController.cs |
| **3** | BlogService.cs, BlogController.cs, ChatSessionService.cs, ChatController.cs | ConversationService.cs, ChatController.cs |
| **4** | ChatModels.cs, AppDbContext.cs, ChatController (+imagen) | Models/*.cs, BankingDbContext.cs |
| **5** | ClimaPlugin.cs, MathPlugin.cs, AgentController.cs | OnboardingPlugin.cs, CalculadoraFinancieraPlugin.cs, OnboardingController.cs |
| **6** | LoggingFilter.cs | LegalPlugin.cs, AuditFilter.cs, PrestamosController.cs, LegalController.cs |
| **7** | PromptingController.cs, ClasificarIntencion.yaml | ClasificarIntencion.yaml, AnalisisDocumento.yaml |
| **8** | DocumentoVectorial.cs, VectorStoreService.cs | VectorStoreService.cs, DocumentoVectorial.cs |
| **9** | RAGController.cs | RAGController.cs, LegalIndexingService.cs, Docs/Leyes/*.txt |
| **10** | appsettings.Production.json | — |
