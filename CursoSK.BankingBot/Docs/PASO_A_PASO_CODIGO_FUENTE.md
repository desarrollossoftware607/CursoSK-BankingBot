# Guía Paso a Paso — Código Fuente del Proyecto Web API

## Semantic Kernel + ASP.NET Core + EF Core (SQLite) + Swagger

Esta guía es para ir **copiando y pegando código** sesión por sesión. Al final tendrás una API completa con ~20 endpoints.

---

## PASO 0 — Crear el Proyecto y Estructura Inicial

### 0.1 Crear el proyecto desde terminal

```bash
dotnet new webapi -n CursoSK.Api --use-controllers
cd CursoSK.Api
```

### 0.2 Instalar todos los paquetes NuGet necesarios

```bash
# Semantic Kernel (base)
dotnet add package Microsoft.SemanticKernel

# Swagger
dotnet add package Swashbuckle.AspNetCore

# Entity Framework Core + SQLite
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design

# Prompt Templates
dotnet add package Microsoft.SemanticKernel.PromptTemplates.Handlebars
dotnet add package Microsoft.SemanticKernel.PromptTemplates.Liquid

# Plugins
dotnet add package Microsoft.SemanticKernel.Plugins.Core --prerelease
dotnet add package Microsoft.SemanticKernel.Plugins.OpenApi --prerelease

# Vector Stores (Sesión 10)
dotnet add package Microsoft.SemanticKernel.Connectors.InMemory --prerelease

# Utilidades
dotnet add package CliWrap
dotnet add package ReverseMarkdown
```

### 0.3 Estructura de carpetas a crear

```bash
mkdir Controllers
mkdir Services
mkdir Plugins
mkdir Filters
mkdir Models
mkdir Data
mkdir DTOs
mkdir Prompts
```

> En Windows PowerShell:
> ```powershell
> "Controllers","Services","Plugins","Filters","Models","Data","DTOs","Prompts" | ForEach-Object { New-Item -ItemType Directory -Name $_ -Force }
> ```

---

## PASO 1 — Modelos de Base de Datos (EF Core + SQLite)

### 1.1 Crear los modelos

Copiar en `Models/ChatModels.cs`:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoSK.Api.Models;

/// <summary>Sesión de chat (un usuario puede tener múltiples sesiones).</summary>
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

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime UltimaActividad { get; set; } = DateTime.UtcNow;

    public bool Activa { get; set; } = true;

    // Navegación
    public List<ChatMessage> Mensajes { get; set; } = new();
}

/// <summary>Mensaje individual dentro de una sesión de chat.</summary>
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
    public string? TipoContenido { get; set; } = "text"; // "text", "image_url", "audio"

    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Navegación
    [ForeignKey(nameof(ChatSessionId))]
    public ChatSession? Session { get; set; }
}

/// <summary>Log de invocaciones de plugins (auditoría).</summary>
public class PluginInvocationLog
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

/// <summary>Contenido generado (blogs, podcasts, etc.).</summary>
public class ContenidoGenerado
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Tipo { get; set; } = "blog"; // "blog", "podcast", "transcripcion"

    [MaxLength(300)]
    public string? Titulo { get; set; }

    public string Contenido { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? UrlImagen { get; set; }

    [MaxLength(500)]
    public string? UrlAudio { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
```

### 1.2 Crear el DbContext

Copiar en `Data/AppDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Models;

namespace CursoSK.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<PluginInvocationLog> PluginInvocationLogs => Set<PluginInvocationLog>();
    public DbSet<ContenidoGenerado> ContenidosGenerados => Set<ContenidoGenerado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Índices
        modelBuilder.Entity<ChatSession>()
            .HasIndex(s => s.SessionId)
            .IsUnique();

        modelBuilder.Entity<ChatSession>()
            .HasIndex(s => s.UsuarioId);

        modelBuilder.Entity<ChatMessage>()
            .HasIndex(m => m.ChatSessionId);

        modelBuilder.Entity<PluginInvocationLog>()
            .HasIndex(l => l.Fecha);

        // Seed Data — Datos de ejemplo
        modelBuilder.Entity<ChatSession>().HasData(
            new ChatSession
            {
                Id = 1,
                SessionId = "demo-session-1",
                Titulo = "Consulta sobre Semantic Kernel",
                UsuarioId = "usuario-demo",
                FechaCreacion = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 1, 8, 30, 0, DateTimeKind.Utc),
                Activa = true
            },
            new ChatSession
            {
                Id = 2,
                SessionId = "demo-session-2",
                Titulo = "Análisis de documento bancario",
                UsuarioId = "usuario-demo",
                FechaCreacion = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 2, 10, 45, 0, DateTimeKind.Utc),
                Activa = true
            },
            new ChatSession
            {
                Id = 3,
                SessionId = "demo-session-3",
                Titulo = "Generación de blog post",
                UsuarioId = "usuario-banco",
                FechaCreacion = new DateTime(2026, 4, 3, 14, 0, 0, DateTimeKind.Utc),
                UltimaActividad = new DateTime(2026, 4, 3, 14, 20, 0, DateTimeKind.Utc),
                Activa = false
            }
        );

        modelBuilder.Entity<ChatMessage>().HasData(
            // Sesión 1: Conversación sobre SK
            new ChatMessage
            {
                Id = 1,
                ChatSessionId = 1,
                Rol = "system",
                Contenido = "Eres un asistente experto en Semantic Kernel y desarrollo .NET.",
                Fecha = new DateTime(2026, 4, 1, 8, 0, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 2,
                ChatSessionId = 1,
                Rol = "user",
                Contenido = "¿Qué es Semantic Kernel y para qué sirve?",
                Fecha = new DateTime(2026, 4, 1, 8, 1, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 3,
                ChatSessionId = 1,
                Rol = "assistant",
                Contenido = "Semantic Kernel es un SDK de código abierto de Microsoft que permite integrar modelos de lenguaje (LLMs) como GPT-4 en aplicaciones C#, Python y Java. Actúa como un orquestador que conecta modelos de IA con plugins, memoria y servicios externos.",
                Fecha = new DateTime(2026, 4, 1, 8, 1, 30, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 4,
                ChatSessionId = 1,
                Rol = "user",
                Contenido = "¿Cómo se configura el Kernel con Azure OpenAI?",
                Fecha = new DateTime(2026, 4, 1, 8, 5, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 5,
                ChatSessionId = 1,
                Rol = "assistant",
                Contenido = "Se usa el patrón Builder:\n\nvar kernel = Kernel.CreateBuilder()\n    .AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)\n    .Build();\n\nDonde deploymentName es el nombre del modelo desplegado en Azure AI Foundry.",
                Fecha = new DateTime(2026, 4, 1, 8, 5, 30, DateTimeKind.Utc)
            },

            // Sesión 2: Documento bancario
            new ChatMessage
            {
                Id = 6,
                ChatSessionId = 2,
                Rol = "system",
                Contenido = "Eres un analista bancario experto en regulaciones y cumplimiento.",
                Fecha = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 7,
                ChatSessionId = 2,
                Rol = "user",
                Contenido = "Analiza este documento de solicitud de crédito y verifica que cumpla con la normativa.",
                TipoContenido = "text",
                Fecha = new DateTime(2026, 4, 2, 10, 1, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 8,
                ChatSessionId = 2,
                Rol = "assistant",
                Contenido = "He analizado el documento. Observaciones:\n1. ✅ Identificación del solicitante completa\n2. ⚠️ Falta firma del codeudor en la página 3\n3. ❌ El monto solicitado excede el límite de la categoría sin respaldo patrimonial\n4. ✅ Documentación fiscal al día",
                Fecha = new DateTime(2026, 4, 2, 10, 2, 0, DateTimeKind.Utc)
            },

            // Sesión 3: Blog post
            new ChatMessage
            {
                Id = 9,
                ChatSessionId = 3,
                Rol = "system",
                Contenido = "Eres un redactor profesional de contenido tecnológico.",
                Fecha = new DateTime(2026, 4, 3, 14, 0, 0, DateTimeKind.Utc)
            },
            new ChatMessage
            {
                Id = 10,
                ChatSessionId = 3,
                Rol = "user",
                Contenido = "Genera un blog post sobre los beneficios de la IA en el sector bancario.",
                Fecha = new DateTime(2026, 4, 3, 14, 1, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<PluginInvocationLog>().HasData(
            new PluginInvocationLog
            {
                Id = 1,
                PluginName = "ClimaPlugin",
                FunctionName = "obtener_clima",
                Argumentos = "{\"ciudad\":\"Tegucigalpa\"}",
                Resultado = "☀️ 28°C, Soleado",
                Fecha = new DateTime(2026, 4, 1, 9, 0, 0, DateTimeKind.Utc),
                DuracionMs = 15
            },
            new PluginInvocationLog
            {
                Id = 2,
                PluginName = "MathPlugin",
                FunctionName = "sumar",
                Argumentos = "{\"a\":15,\"b\":27}",
                Resultado = "42",
                Fecha = new DateTime(2026, 4, 1, 9, 5, 0, DateTimeKind.Utc),
                DuracionMs = 2
            }
        );

        modelBuilder.Entity<ContenidoGenerado>().HasData(
            new ContenidoGenerado
            {
                Id = 1,
                Tipo = "blog",
                Titulo = "Introducción a Semantic Kernel",
                Contenido = "<!-- wp:paragraph --><p>Semantic Kernel es un SDK revolucionario...</p><!-- /wp:paragraph -->",
                UrlImagen = "https://example.com/imagen-sk.png",
                FechaCreacion = new DateTime(2026, 4, 3, 14, 20, 0, DateTimeKind.Utc)
            }
        );
    }
}
```

### 1.3 Crear la migración inicial

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Esto crea el archivo `cursosk.db` (SQLite) con todas las tablas y datos de ejemplo.

---

## PASO 2 — DTOs (Data Transfer Objects)

Copiar en `DTOs/Requests.cs`:

```csharp
namespace CursoSK.Api.DTOs;

// --- Sesión 1: Kernel ---
public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);

// --- Sesión 2: Multimodal ---
public record ImagenRequest(string Descripcion, string? Quality = "hd", string? Style = "vivid");
public record TTSRequest(string Texto, string? Voice = "alloy");

// --- Sesión 3: Blog ---
public record BlogRequest(string Tema);
public record BlogPublicarRequest(string Tema, string WpUrl, string WpUser, string WpAppPassword);

// --- Sesión 4: Chat ---
public record ChatMensajeRequest(string Mensaje);
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);

// --- Sesión 6: OpenAPI ---
public record OpenApiPluginRequest(string Nombre, string SpecUrl);

// --- Sesión 8: Prompting ---
public record TextoRequest(string Texto);
public record TraducirRequest(string Texto, string IdiomaOrigen, string IdiomaDestino, string? TemplateFormat);

// --- Sesión 9: Podcast ---
public record PodcastRequest(string[] Urls, string Tema, string Formato = "Conversacional");

// --- Sesión 10: RAG ---
public record FAQRequest(string Pregunta, string Respuesta, string Categoria);
```

---

## PASO 3 — Program.cs Completo

Reemplazar el contenido de `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using CursoSK.Api.Data;
using CursoSK.Api.Plugins;
using CursoSK.Api.Filters;
using CursoSK.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// --- ASP.NET Core ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Curso SK - API de IA", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml"), true);
});

// --- Entity Framework + SQLite ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));

// --- HttpClient ---
builder.Services.AddHttpClient();

// --- Semantic Kernel ---
#pragma warning disable SKEXP0010, SKEXP0070
var llmProvider = builder.Configuration["LLMSettings:Provider"]?.ToLower() ?? "azure";
var kernelBuilder = Kernel.CreateBuilder();

if (llmProvider == "openai")
{
    var apiKey = builder.Configuration["LLMSettings:OpenAI:ApiKey"]!;
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: builder.Configuration["LLMSettings:OpenAI:ModelId"] ?? "gpt-4o-mini",
        apiKey: apiKey);
    // Servicios multimodales (Sesión 2)
    kernelBuilder.AddOpenAITextToImage(apiKey: apiKey, modelId: "dall-e-3");
    kernelBuilder.AddOpenAITextToAudio(apiKey: apiKey, modelId: "tts-1");
    kernelBuilder.AddOpenAIAudioToText(apiKey: apiKey, modelId: "whisper-1");
    // Embeddings (Sesión 10)
    kernelBuilder.AddOpenAITextEmbeddingGeneration("text-embedding-ada-002", apiKey);
}
else // azure
{
    var endpoint = builder.Configuration["LLMSettings:AzureOpenAI:Endpoint"]!;
    var apiKey = builder.Configuration["LLMSettings:AzureOpenAI:ApiKey"]!;
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: builder.Configuration["LLMSettings:AzureOpenAI:DeploymentName"] ?? "gpt-4o-mini",
        endpoint: endpoint,
        apiKey: apiKey);
    // Nota: Para DALL-E, TTS y Whisper en Azure, requieres deployments separados
    // kernelBuilder.AddAzureOpenAITextToImage(...);
}

// Filtro de logging (Sesión 6)
kernelBuilder.Services.AddLogging();
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter>(sp =>
    new LoggingFilter(sp.GetRequiredService<ILogger<LoggingFilter>>()));
#pragma warning restore SKEXP0010, SKEXP0070

var kernel = kernelBuilder.Build();

// Registrar Plugins (Sesiones 5-7)
kernel.Plugins.AddFromObject(new ClimaPlugin(), "ClimaPlugin");
kernel.Plugins.AddFromObject(new MathPlugin(), "MathPlugin");
kernel.Plugins.AddFromObject(new WikipediaPlugin(new HttpClient()), "WikipediaPlugin");
kernel.Plugins.AddFromObject(new VideoPlugin(), "VideoPlugin");

builder.Services.AddSingleton(kernel);

// --- Servicios (Sesiones 3, 4, 9, 10) ---
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<ChatSessionService>();
builder.Services.AddSingleton<PodcastService>();
builder.Services.AddSingleton<VectorStoreService>();

var app = builder.Build();

// Asegurar que la BD existe y tiene seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// --- Pipeline HTTP ---
app.UseSwagger();
app.UseSwaggerUI(c => c.DocumentTitle = "Curso SK - Swagger");
app.MapControllers();

app.Run();
```

### 3.1 Configurar `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
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
      "DeploymentName": "gpt-4o-mini",
      "Endpoint": "https://tu-recurso.openai.azure.com",
      "ApiKey": "TU-API-KEY-AQUI"
    }
  }
}
```

### 3.2 Habilitar XML docs para Swagger

Agregar al archivo `.csproj`:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

---

## PASO 4 — Controladores (copiar uno por sesión)

### 4.1 KernelController.cs (Sesión 1)

Copiar en `Controllers/KernelController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("1. Kernel Básico")]
public class KernelController : ControllerBase
{
    private readonly Kernel _kernel;
    public KernelController(Kernel kernel) => _kernel = kernel;

    /// <summary>Genera texto con un prompt libre.</summary>
    [HttpPost("prompt")]
    public async Task<IActionResult> InvokePrompt([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { respuesta = result.ToString() });
    }

    /// <summary>Genera texto con parámetros configurables (temperatura, max_tokens).</summary>
    [HttpPost("prompt/configurado")]
    public async Task<IActionResult> InvokePromptConSettings([FromBody] PromptConSettingsRequest request)
    {
        var settings = new OpenAIPromptExecutionSettings
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(settings));
        return Ok(new { respuesta = result.ToString(), maxTokens = request.MaxTokens, temperature = request.Temperature });
    }
}
```

### 4.2 MultimodalController.cs (Sesión 2)

Copiar en `Controllers/MultimodalController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AudioToText;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToAudio;
using Microsoft.SemanticKernel.TextToImage;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("2. Multimodal")]
public class MultimodalController : ControllerBase
{
    private readonly Kernel _kernel;
    public MultimodalController(Kernel kernel) => _kernel = kernel;

    /// <summary>Chat Completion con streaming (Server-Sent Events).</summary>
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

    /// <summary>Genera una imagen con DALL-E.</summary>
    [HttpPost("imagen")]
    public async Task<IActionResult> GenerarImagen([FromBody] ImagenRequest request)
    {
        #pragma warning disable SKEXP0010
        var imageService = _kernel.GetRequiredService<ITextToImageService>();
        var images = await imageService.GetImageContentsAsync(request.Descripcion,
            new OpenAITextToImageExecutionSettings
            {
                Size = (1792, 1024),
                Quality = request.Quality ?? "hd",
                Style = request.Style ?? "vivid"
            });
        return Ok(new { url = images[0].Uri!.ToString() });
        #pragma warning restore SKEXP0010
    }

    /// <summary>Convierte texto a audio MP3.</summary>
    [HttpPost("tts")]
    public async Task<IActionResult> TextToSpeech([FromBody] TTSRequest request)
    {
        #pragma warning disable SKEXP0010
        var audioService = _kernel.GetRequiredService<ITextToAudioService>();
        var audio = await audioService.GetAudioContentAsync(request.Texto,
            new OpenAITextToAudioExecutionSettings("tts-1") { Voice = request.Voice ?? "alloy" });
        return File(audio.Data!.ToArray(), "audio/mpeg", "audio.mp3");
        #pragma warning restore SKEXP0010
    }

    /// <summary>Transcribe un archivo de audio a texto (Whisper).</summary>
    [HttpPost("stt")]
    public async Task<IActionResult> SpeechToText(IFormFile archivo)
    {
        #pragma warning disable SKEXP0010
        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms);
        var whisper = _kernel.GetRequiredService<IAudioToTextService>();
        var result = await whisper.GetTextContentAsync(new AudioContent(ms.ToArray(), archivo.ContentType));
        return Ok(new { texto = result.Text });
        #pragma warning restore SKEXP0010
    }
}
```

### 4.3 ChatController.cs (Sesión 4) — Con persistencia en SQLite

Copiar en `Controllers/ChatController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using CursoSK.Api.Data;
using CursoSK.Api.DTOs;
using CursoSK.Api.Models;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("4. Chat con Historia")]
public class ChatController : ControllerBase
{
    private readonly Kernel _kernel;
    private readonly AppDbContext _db;

    public ChatController(Kernel kernel, AppDbContext db)
    {
        _kernel = kernel;
        _db = db;
    }

    /// <summary>Envía un mensaje a una sesión de chat (crea la sesión si no existe).</summary>
    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
    {
        // Buscar o crear sesión en BD
        var session = await _db.ChatSessions
            .Include(s => s.Mensajes)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session == null)
        {
            session = new ChatSession
            {
                SessionId = sessionId,
                Titulo = request.Mensaje.Length > 50 ? request.Mensaje[..50] + "..." : request.Mensaje
            };
            _db.ChatSessions.Add(session);
            await _db.SaveChangesAsync();

            // Agregar mensaje de sistema
            _db.ChatMessages.Add(new ChatMessage
            {
                ChatSessionId = session.Id,
                Rol = "system",
                Contenido = "Eres un asistente útil que recuerda toda la conversación."
            });
        }

        // Guardar mensaje del usuario
        _db.ChatMessages.Add(new ChatMessage
        {
            ChatSessionId = session.Id,
            Rol = "user",
            Contenido = request.Mensaje
        });
        await _db.SaveChangesAsync();

        // Reconstruir ChatHistory desde BD
        var mensajes = await _db.ChatMessages
            .Where(m => m.ChatSessionId == session.Id)
            .OrderBy(m => m.Fecha)
            .ToListAsync();

        var chatHistory = new ChatHistory();
        foreach (var m in mensajes)
        {
            switch (m.Rol)
            {
                case "system": chatHistory.AddSystemMessage(m.Contenido); break;
                case "user": chatHistory.AddUserMessage(m.Contenido); break;
                case "assistant": chatHistory.AddAssistantMessage(m.Contenido); break;
            }
        }

        // Obtener respuesta del LLM
        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(chatHistory);

        // Guardar respuesta en BD
        _db.ChatMessages.Add(new ChatMessage
        {
            ChatSessionId = session.Id,
            Rol = "assistant",
            Contenido = response.Content!
        });
        session.UltimaActividad = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(new { sessionId, respuesta = response.Content });
    }

    /// <summary>Envía una imagen (URL) al chat para análisis multimodal.</summary>
    [HttpPost("{sessionId}/imagen")]
    public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
    {
        var session = await _db.ChatSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
        if (session == null) return NotFound("Sesión no encontrada. Envíe un mensaje primero.");

        // Guardar referencia del mensaje con imagen
        _db.ChatMessages.Add(new ChatMessage
        {
            ChatSessionId = session.Id,
            Rol = "user",
            Contenido = $"[Imagen: {request.ImagenUrl}] {request.Pregunta ?? "Describe esta imagen"}",
            TipoContenido = "image_url"
        });
        await _db.SaveChangesAsync();

        // Construir mensaje multimodal
        var chatHistory = new ChatHistory();
        var mensajes = await _db.ChatMessages.Where(m => m.ChatSessionId == session.Id).OrderBy(m => m.Fecha).ToListAsync();
        foreach (var m in mensajes.Where(m => m.TipoContenido == "text"))
        {
            switch (m.Rol)
            {
                case "system": chatHistory.AddSystemMessage(m.Contenido); break;
                case "user": chatHistory.AddUserMessage(m.Contenido); break;
                case "assistant": chatHistory.AddAssistantMessage(m.Contenido); break;
            }
        }

        var contents = new ChatMessageContentItemCollection();
        contents.Add(new TextContent(request.Pregunta ?? "Describe esta imagen en detalle"));
        contents.Add(new ImageContent(new Uri(request.ImagenUrl)));
        chatHistory.AddUserMessage(contents);

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(chatHistory);

        _db.ChatMessages.Add(new ChatMessage { ChatSessionId = session.Id, Rol = "assistant", Contenido = response.Content! });
        await _db.SaveChangesAsync();

        return Ok(new { sessionId, respuesta = response.Content });
    }

    /// <summary>Obtiene el historial completo de una sesión.</summary>
    [HttpGet("{sessionId}/historial")]
    public async Task<IActionResult> ObtenerHistorial(string sessionId)
    {
        var session = await _db.ChatSessions
            .Include(s => s.Mensajes.OrderBy(m => m.Fecha))
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        if (session == null) return NotFound();
        return Ok(new
        {
            session.SessionId,
            session.Titulo,
            session.FechaCreacion,
            mensajes = session.Mensajes.Select(m => new { m.Rol, m.Contenido, m.Fecha })
        });
    }

    /// <summary>Lista todas las sesiones de chat.</summary>
    [HttpGet("sesiones")]
    public async Task<IActionResult> ListarSesiones()
    {
        var sesiones = await _db.ChatSessions
            .OrderByDescending(s => s.UltimaActividad)
            .Select(s => new
            {
                s.SessionId,
                s.Titulo,
                s.FechaCreacion,
                s.UltimaActividad,
                s.Activa,
                totalMensajes = s.Mensajes.Count
            })
            .ToListAsync();
        return Ok(sesiones);
    }

    /// <summary>Elimina una sesión y todos sus mensajes.</summary>
    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> EliminarSesion(string sessionId)
    {
        var session = await _db.ChatSessions
            .Include(s => s.Mensajes)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        if (session == null) return NotFound();
        _db.ChatMessages.RemoveRange(session.Mensajes);
        _db.ChatSessions.Remove(session);
        await _db.SaveChangesAsync();
        return Ok(new { mensaje = "Sesión eliminada" });
    }
}
```

### 4.4 AgentController.cs (Sesiones 5-6)

Copiar en `Controllers/AgentController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("5-6. Agente con Plugins")]
public class AgentController : ControllerBase
{
    private readonly Kernel _kernel;
    public AgentController(Kernel kernel) => _kernel = kernel;

    /// <summary>Consulta al agente con Function Calling automático (invoca plugins según necesidad).</summary>
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

    /// <summary>Lista todos los plugins y funciones registrados.</summary>
    [HttpGet("plugins")]
    public IActionResult ListarPlugins()
    {
        var plugins = _kernel.Plugins.Select(p => new
        {
            nombre = p.Name,
            funciones = p.Select(f => new
            {
                nombre = f.Name,
                descripcion = f.Description,
                parametros = f.Metadata.Parameters.Select(p => new { p.Name, p.Description, tipo = p.ParameterType?.Name })
            })
        });
        return Ok(plugins);
    }

    /// <summary>Importa un plugin desde una especificación OpenAPI.</summary>
    [HttpPost("plugins/openapi")]
    public async Task<IActionResult> CargarPluginOpenApi([FromBody] OpenApiPluginRequest request)
    {
        #pragma warning disable SKEXP0040
        await _kernel.ImportPluginFromOpenApiAsync(request.Nombre, new Uri(request.SpecUrl),
            new OpenApiFunctionExecutionParameters { EnablePayloadNamespacing = true });
        #pragma warning restore SKEXP0040
        return Ok(new { mensaje = $"Plugin '{request.Nombre}' cargado" });
    }
}
```

---

## PASO 5 — Plugins

### 5.1 ClimaPlugin.cs

Copiar en `Plugins/ClimaPlugin.cs`:

```csharp
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class ClimaPlugin
{
    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad específica")]
    public string ObtenerClima([Description("Nombre de la ciudad")] string ciudad)
    {
        var climas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "tegucigalpa", "☀️ 28°C, Soleado" },
            { "san pedro sula", "🌤️ 32°C, Parcialmente nublado" },
            { "la ceiba", "🌧️ 26°C, Lluvioso" },
            { "ciudad de mexico", "⛅ 22°C, Nublado" },
            { "bogota", "🌧️ 14°C, Lluvioso" }
        };
        return climas.GetValueOrDefault(ciudad, $"No tengo datos para {ciudad}");
    }
}
```

### 5.2 MathPlugin.cs

Copiar en `Plugins/MathPlugin.cs`:

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
    [Description("Resta dos números")]
    public double Restar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a - b;

    [KernelFunction("multiplicar")]
    [Description("Multiplica dos números")]
    public double Multiplicar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a * b;

    [KernelFunction("dividir")]
    [Description("Divide dos números")]
    public double Dividir([Description("Dividendo")] double a, [Description("Divisor")] double b) =>
        b == 0 ? throw new DivideByZeroException("No se puede dividir entre cero") : a / b;
}
```

### 5.3 WikipediaPlugin.cs

Copiar en `Plugins/WikipediaPlugin.cs`:

```csharp
using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class WikipediaPlugin
{
    private readonly HttpClient _httpClient;
    public WikipediaPlugin(HttpClient httpClient) => _httpClient = httpClient;

    [KernelFunction("buscar_en_wikipedia")]
    [Description("Busca información en Wikipedia sobre un tema")]
    public async Task<string> BuscarEnWikipedia([Description("Tema a buscar")] string tema)
    {
        var url = $"https://es.wikipedia.org/api/rest_v1/page/summary/{Uri.EscapeDataString(tema)}";
        var response = await _httpClient.GetStringAsync(url);
        var json = JsonSerializer.Deserialize<JsonElement>(response);
        return json.GetProperty("extract").GetString() ?? "No encontrado";
    }
}
```

### 5.4 VideoPlugin.cs

Copiar en `Plugins/VideoPlugin.cs`:

```csharp
using System.ComponentModel;
using CliWrap;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class VideoPlugin
{
    [KernelFunction("extraer_audio")]
    [Description("Extrae el audio de un archivo de video")]
    public async Task<string> ExtraerAudio(
        [Description("Ruta del video")] string videoPath,
        [Description("Ruta de salida del audio")] string outputPath)
    {
        await Cli.Wrap("ffmpeg").WithArguments($"-i \"{videoPath}\" -vn -acodec libmp3lame \"{outputPath}\" -y").ExecuteAsync();
        return $"Audio extraído: {outputPath}";
    }

    [KernelFunction("comprimir_audio")]
    [Description("Comprime un archivo de audio para Whisper (< 25MB)")]
    public async Task<string> ComprimirAudio(
        [Description("Ruta del audio")] string audioPath,
        [Description("Ruta de salida")] string outputPath)
    {
        await Cli.Wrap("ffmpeg").WithArguments($"-i \"{audioPath}\" -b:a 32k -ar 16000 -ac 1 \"{outputPath}\" -y").ExecuteAsync();
        return $"Audio comprimido: {outputPath}";
    }

    [KernelFunction("cortar_clip")]
    [Description("Corta un segmento de un video")]
    public async Task<string> CortarClip(
        [Description("Ruta del video")] string videoPath,
        [Description("Inicio (HH:MM:SS)")] string inicio,
        [Description("Duración (HH:MM:SS)")] string duracion,
        [Description("Ruta de salida")] string outputPath)
    {
        await Cli.Wrap("ffmpeg").WithArguments($"-i \"{videoPath}\" -ss {inicio} -t {duracion} -c copy \"{outputPath}\" -y").ExecuteAsync();
        return $"Clip generado: {outputPath}";
    }
}
```

---

## PASO 6 — Filtros

Copiar en `Filters/LoggingFilter.cs`:

```csharp
using System.Text.Json;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Filters;

public class LoggingFilter : IFunctionInvocationFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger) => _logger = logger;

    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        _logger.LogInformation("🔧 Plugin: {Plugin}.{Function} | Args: {Args}",
            context.Function.PluginName, context.Function.Name,
            JsonSerializer.Serialize(context.Arguments));

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        _logger.LogInformation("✅ {Plugin}.{Function} → {Duration}ms | Resultado: {Result}",
            context.Function.PluginName, context.Function.Name,
            sw.ElapsedMilliseconds, context.Result?.ToString()?[..Math.Min(200, context.Result?.ToString()?.Length ?? 0)]);
    }
}
```

---

## PASO 7 — Servicios

### 7.1 BlogService.cs (Sesión 3)

Copiar en `Services/BlogService.cs`:

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToImage;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace CursoSK.Api.Services;

public class BlogService
{
    private readonly Kernel _kernel;
    public BlogService(Kernel kernel) => _kernel = kernel;

    public async Task<string> GenerarContenidoBlog(string tema)
    {
        var prompt = $"""
            Genera una publicación de blog detallada acerca de {tema}.
            Incluye introducción, párrafos, code snippets y conclusión.
            Usa bloques Gutenberg de WordPress:
            <!-- wp:heading --><h2 class="wp-block-heading">TEXTO</h2><!-- /wp:heading -->
            <!-- wp:paragraph --><p>TEXTO</p><!-- /wp:paragraph -->
            <!-- wp:code --><pre class="wp-block-code"><code>CÓDIGO</code></pre><!-- /wp:code -->
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return result.ToString();
    }

    public async Task<string> GenerarImagenDestacada(string tema)
    {
        #pragma warning disable SKEXP0010
        var imageService = _kernel.GetRequiredService<ITextToImageService>();
        var images = await imageService.GetImageContentsAsync(
            $"Imagen profesional para blog sobre: {tema}",
            new OpenAITextToImageExecutionSettings { Size = (1792, 1024), Quality = "hd" });
        return images[0].Uri!.ToString();
        #pragma warning restore SKEXP0010
    }
}
```

### 7.2 ChatSessionService.cs (Sesión 4) — En memoria (respaldo)

Copiar en `Services/ChatSessionService.cs`:

```csharp
using System.Collections.Concurrent;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CursoSK.Api.Services;

/// <summary>Servicio de chat en memoria (usado como respaldo si no se necesita persistencia).</summary>
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
}
```

### 7.3 PodcastService.cs (Sesión 9)

Copiar en `Services/PodcastService.cs`:

```csharp
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace CursoSK.Api.Services;

public class PodcastService
{
    private readonly Kernel _kernel;
    private readonly IHttpClientFactory _httpClientFactory;

    public PodcastService(Kernel kernel, IHttpClientFactory httpClientFactory)
    {
        _kernel = kernel;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> DescargarYConvertir(string[] urls)
    {
        var converter = new ReverseMarkdown.Converter();
        var client = _httpClientFactory.CreateClient();
        var sb = new StringBuilder();
        foreach (var url in urls)
        {
            var html = await client.GetStringAsync(url.Trim());
            sb.AppendLine(converter.Convert(html));
        }
        return sb.ToString();
    }

    public async Task<string> GenerarScript(string tema, string contenido, string formato)
    {
        var prompt = $"""
            Eres un productor de podcasts. Genera un script completo para un podcast
            en formato {formato} sobre: {tema}
            
            Contenido de referencia: {contenido[..Math.Min(3000, contenido.Length)]}
            
            Incluye: introducción, puntos clave, ejemplos y conclusión.
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return result.ToString();
    }

    public async Task<byte[]> GenerarAudio(string script)
    {
        #pragma warning disable SKEXP0010
        var audioService = _kernel.GetRequiredService<Microsoft.SemanticKernel.TextToAudio.ITextToAudioService>();
        var audio = await audioService.GetAudioContentAsync(script,
            new OpenAITextToAudioExecutionSettings("tts-1-hd") { Voice = "nova", Speed = 1.0f });
        return audio.Data!.ToArray();
        #pragma warning restore SKEXP0010
    }
}
```

### 7.4 VectorStoreService.cs (Sesión 10)

Copiar en `Services/VectorStoreService.cs`:

```csharp
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.InMemory;
using Microsoft.SemanticKernel.Data;
using Microsoft.SemanticKernel.Embeddings;
using CursoSK.Api.Models;

namespace CursoSK.Api.Services;

public class VectorStoreService
{
    private readonly Kernel _kernel;
    private readonly IVectorStoreRecordCollection<string, DocumentoFAQ> _collection;

    public VectorStoreService(Kernel kernel)
    {
        _kernel = kernel;
        var store = new InMemoryVectorStore();
        _collection = store.GetCollection<string, DocumentoFAQ>("faq");
        _collection.CreateCollectionIfNotExistsAsync().GetAwaiter().GetResult();
    }

    public async Task AgregarFAQ(string pregunta, string respuesta, string categoria)
    {
        #pragma warning disable SKEXP0010
        var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var embedding = await embeddingService.GenerateEmbeddingAsync(pregunta);
        await _collection.UpsertAsync(new DocumentoFAQ
        {
            Id = Guid.NewGuid().ToString(),
            Pregunta = pregunta,
            Respuesta = respuesta,
            Categoria = categoria,
            Embedding = embedding
        });
        #pragma warning restore SKEXP0010
    }

    public async Task<List<(DocumentoFAQ Doc, double? Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        #pragma warning disable SKEXP0010
        var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
        var queryEmbedding = await embeddingService.GenerateEmbeddingAsync(consulta);
        var results = await _collection.VectorizedSearchAsync(queryEmbedding, new VectorSearchOptions { Top = top });
        var items = new List<(DocumentoFAQ, double?)>();
        await foreach (var r in results.Results)
            items.Add((r.Record, r.Score));
        return items;
        #pragma warning restore SKEXP0010
    }

    public IVectorStoreRecordCollection<string, DocumentoFAQ> Collection => _collection;
}
```

---

## PASO 8 — Modelo para Vector Store

Copiar en `Models/DocumentoFAQ.cs`:

```csharp
using Microsoft.Extensions.VectorData;

namespace CursoSK.Api.Models;

public class DocumentoFAQ
{
    [VectorStoreRecordKey]
    public string Id { get; set; } = string.Empty;

    [VectorStoreRecordData]
    public string Pregunta { get; set; } = string.Empty;

    [VectorStoreRecordData]
    public string Respuesta { get; set; } = string.Empty;

    [VectorStoreRecordData]
    public string Categoria { get; set; } = string.Empty;

    [VectorStoreRecordVector(1536)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
```

---

## PASO 9 — Archivo YAML de Prompt (Sesión 8)

Copiar en `Prompts/ClasificarIntencion.yaml`:

```yaml
name: ClasificarIntencion
description: Clasifica la intención del usuario
template_format: handlebars
template: |
  <message role="system">
  Clasifica la siguiente consulta en UNA categoría:
  - consulta_precio: preguntas sobre precios o costos
  - agendar_cita: solicitudes de citas o agendamiento  
  - informacion: preguntas generales
  - reclamo: quejas o reclamos
  Responde SOLO con el nombre de la categoría.
  </message>
  <message role="user">{{consulta}}</message>
execution_settings:
  default:
    max_tokens: 50
    temperature: 0.0
input_variables:
  - name: consulta
    description: La consulta del usuario
    is_required: true
```

---

## PASO 10 — Ejecutar y Probar

```bash
# Ejecutar el proyecto
dotnet run

# Abrir en el navegador:
# https://localhost:5001/swagger
```

### Orden sugerido para probar en Swagger:

1. **GET** `/api/chat/sesiones` → ver las 3 sesiones seed
2. **GET** `/api/chat/demo-session-1/historial` → ver conversación de ejemplo
3. **POST** `/api/kernel/prompt` → `{ "prompt": "Hola, ¿qué es Semantic Kernel?" }`
4. **POST** `/api/chat/mi-sesion/mensaje` → `{ "mensaje": "Mi nombre es José" }`
5. **POST** `/api/chat/mi-sesion/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }`
6. **GET** `/api/agent/plugins` → ver todos los plugins registrados
7. **POST** `/api/agent/consultar` → `{ "prompt": "¿Qué clima hace en Tegucigalpa?" }`

---

## Diagrama de Tablas (SQLite)

```
┌─────────────────────┐       ┌──────────────────────┐
│    ChatSessions      │       │    ChatMessages       │
├─────────────────────┤       ├──────────────────────┤
│ Id (PK)             │──┐    │ Id (PK)              │
│ SessionId (unique)  │  │    │ ChatSessionId (FK)   │
│ Titulo              │  └───>│ Rol                  │
│ UsuarioId           │       │ Contenido            │
│ FechaCreacion       │       │ TipoContenido        │
│ UltimaActividad     │       │ Fecha                │
│ Activa              │       └──────────────────────┘
└─────────────────────┘
                              ┌──────────────────────┐
┌─────────────────────┐       │  ContenidoGenerado   │
│ PluginInvocationLog │       ├──────────────────────┤
├─────────────────────┤       │ Id (PK)              │
│ Id (PK)             │       │ Tipo                 │
│ PluginName          │       │ Titulo               │
│ FunctionName        │       │ Contenido            │
│ Argumentos          │       │ UrlImagen            │
│ Resultado           │       │ UrlAudio             │
│ Fecha               │       │ FechaCreacion        │
│ DuracionMs          │       └──────────────────────┘
└─────────────────────┘
```
