# Curso: Máster en Semantic Kernel — Creando Proyectos con IA

## Información General

| Campo | Detalle |
|-------|---------|
| **Nombre del Curso** | Máster en Semantic Kernel creando proyectos con IA |
| **Duración** | 30 horas cronológicas (5 semanas) |
| **Horario** | Lunes, Miércoles y Viernes — 3 horas por sesión (10 sesiones) |
| **Modalidad** | Formación práctica de alto nivel — "Aprender Haciendo" |
| **Requisitos** | Conocimientos básicos de C# / .NET, Visual Studio 2022 o VS Code, cuenta de OpenAI o suscripción Azure OpenAI |
| **Enfoque del Proyecto** | **Una sola ASP.NET Core Web API con Swagger** que crece incrementalmente sesión a sesión — cada clase agrega nuevos endpoints al mismo proyecto |
| **Basado en** | Curso Udemy "Máster en Semantic Kernel creando proyectos" por Héctor Uriel Pérez (8h 28m, 75 lecciones, 10 secciones) |
| **Idioma** | Español |

---

## Objetivo General

Al finalizar el curso, los participantes serán capaces de **construir una API REST inteligente con ASP.NET Core, Swagger y Semantic Kernel** que integre modelos de IA (OpenAI / Azure OpenAI) para: generar texto, imágenes y audio; crear plugins nativos con Function Calling; implementar técnicas avanzadas de prompting; y utilizar Vector Stores para búsqueda semántica — todo desarrollado sobre **un único proyecto Web API que crece sesión a sesión**, probando cada endpoint desde Swagger.

---

## Competencias a Desarrollar

| # | Competencia |
|---|-------------|
| 1 | Configurar Kernels de Semantic Kernel con OpenAI y Azure OpenAI |
| 2 | Utilizar Chat Completion, Streaming, generación de imágenes (DALL-E) y audio (TTS/Whisper) |
| 3 | Crear plugins nativos en C# con `[KernelFunction]` y `[Description]` |
| 4 | Implementar Function Calling automático y manual |
| 5 | Diseñar prompts con plantillas Handlebars, Liquid y YAML |
| 6 | Integrar FFmpeg para procesamiento de video/audio |
| 7 | Implementar Vector Stores, embeddings y búsqueda semántica |
| 8 | Exponer todas las capacidades como endpoints REST documentados con Swagger |
| 9 | Construir un proyecto Web API incremental con módulos: Blog, Chat, Video, Podcast, RAG |

---

## Contenido del Curso (Secciones Udemy → Sesiones de Clase)

| Sección Udemy | Título | Lecciones | Duración | Sesión(es) |
|:---:|---------|:---------:|:--------:|:----------:|
| 1 | Introducción | 4 | 12 min | Sesión 1 |
| 2 | El Kernel — Core Engine | 12 | 1h 08m | Sesiones 1-2 |
| 3 | Workshop: Blog Posts con SK | 7 | 39 min | Sesión 3 |
| 4 | Experimentando con Chat Completion | 3 | 20 min | Sesión 4 |
| 5 | Workshop: Chat Multi-modal | 5 | 33 min | Sesión 4 |
| 6 | Plugins | 13 | 1h 32m | Sesiones 5-6 |
| 7 | Workshop: Extracción de datos de video | 8 | 1h 07m | Sesión 7 |
| 8 | Prompting | 7 | 44 min | Sesión 8 |
| 9 | Workshop: Generador de Podcasts | 8 | 1h 02m | Sesión 9 |
| 10 | Memoria (Vector Stores) y Búsqueda por Texto | 8 | 1h 10m | Sesión 10 |

---

## Proyecto de Referencia en Producción

El curso se complementa con un **sistema real en producción**: un agente conversacional para laboratorio clínico (WhatsApp/Messenger) que demuestra los mismos conceptos a escala empresarial:

| Concepto del Curso | Implementación en Producción | Archivo de Referencia |
|---------------------|------------------------------|----------------------|
| Creación del Kernel con Azure OpenAI | `Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(...)` + Multi-LLM (ChatGPT/Gemini) | `Program.cs` (líneas 330-380) |
| Plugins nativos con `[KernelFunction]` | 6 plugins: Cotización, Citas, Domicilio, Análisis, Laboratorio, FechaHora | `API/Plugins/*.cs` |
| Function Calling automático | `ToolCallBehavior.AutoInvokeKernelFunctions` con 20+ funciones | `CotizacionIntegration.cs` |
| Chat History y estado conversacional | `ConversationSession` por número de teléfono + `ChatHistory` | `CotizacionIntegration.cs` |
| Prompt Templates con variables | `skprompt.txt` + `config.json` (DeterminarIntencion, BuscarAnalisis) | `API/Prompts/LabAnalisis/` |
| Transcripción de audio (Whisper) | Azure OpenAI `gpt-4o-transcribe` vía REST multipart | `CotizacionIntegration.TranscribeAudio()` |
| Configuración de PromptExecutionSettings | `OpenAIPromptExecutionSettings` con temperatura, max_tokens | `CotizacionIntegration.CreatePromptExecutionSettings()` |

---

## Sílabo Detallado por Lección

### SECCIÓN 1: Introducción (4 lecciones — 12 min)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 1 | ¿Qué es Semantic Kernel? | Video | 4:40 |
| 2 | Componentes de Semantic Kernel | Video | 1:06 |
| 3 | Beneficios y Casos de Uso | Video | 2:34 |
| 4 | Configurando un proyecto en VS 2022 con Semantic Kernel | Video | 4:00 |

**Resumen:** Semantic Kernel es un SDK de código abierto de Microsoft que permite integrar LLMs (OpenAI, Azure OpenAI, Hugging Face) en aplicaciones C#, Python y Java. El Kernel actúa como orquestador central que coordina modelos de IA, plugins y servicios externos. Se configura un proyecto inicial en Visual Studio 2022 instalando el paquete NuGet `Microsoft.SemanticKernel`.

### SECCIÓN 2: El Kernel — Core Engine (12 lecciones — 1h 08m)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 5 | Comprendiendo al Kernel como orquestador | Video | 1:54 |
| 6 | El patrón Builder | Video | 2:45 |
| 7 | Demostración — El Patrón Builder | Video | 3:55 |
| 8 | Creando una API Key para conectarnos a OpenAI | Video | 2:57 |
| 9 | Creando una API Key para conectarnos a Azure OpenAI | Video | 3:27 |
| 10 | Creando el proyecto, variables de entorno y Kernels | Video | 9:46 |
| 11 | Chat Completion usando Semantic Kernel | Video | 5:45 |
| 12 | Chat Completion Streaming usando Semantic Kernel | Video | 3:50 |
| 13 | Generando imágenes utilizando Semantic Kernel | Video | 14:22 |
| 14 | Nota sobre la generación de imágenes | Artículo | — |
| 15 | Generando archivos de audio utilizando Semantic Kernel | Video | 11:16 |
| 16 | Extrayendo texto de audio utilizando Semantic Kernel | Video | 8:14 |

**Resumen:** Se construyen dos Kernels (OpenAI y Azure OpenAI) usando el patrón Builder. Se demuestra `InvokePromptAsync()` para Chat Completion, streaming con `GetStreamingChatMessageContentsAsync()`, generación de imágenes con DALL-E (`AddOpenAITextToImage`), Text-to-Speech (`AddOpenAITextToAudio`) y Audio-to-Text con Whisper (`AddOpenAIAudioToText`). Se configuran `OpenAIPromptExecutionSettings` para controlar temperatura, max_tokens, etc.

### SECCIÓN 3: Workshop — Creating Blog Posts using SK (7 lecciones — 39 min)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 17 | Acerca del proyecto | Video | 1:33 |
| 18 | Creando el proyecto y configurando los kernels | Video | 3:00 |
| 19 | Generando la entrada del blog | Video | 13:09 |
| 20 | Generando una imagen destacada para la entrada del blog | Video | 7:14 |
| 21 | Generando el archivo de audio de la entrada del blog | Video | 7:01 |
| 22 | Publicando el contenido en WordPress | Video | 6:44 |
| 23 | Descarga el Proyecto Completado | Artículo | — |

**Resumen:** Proyecto completo que genera publicaciones de blog con bloques Gutenberg de WordPress. Se crea un prompt que genera contenido HTML con etiquetas Gutenberg (headings, paragraphs, lists, code blocks). Se genera imagen destacada con DALL-E y audio TTS. Se publica automáticamente en WordPress vía API REST. Se usa la librería Spectre.Console para UI interactiva en consola.

### SECCIÓN 4: Experimentando con Chat Completion (3 lecciones — 20 min)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 24 | Usando el servicio Chat Completion | Video | 4:20 |
| 25 | Añadiendo historial del chat | Video | 8:51 |
| 26 | Chat Completion multi-modal | Video | 7:18 |

**Resumen:** Se trabaja directamente con `IChatCompletionService` vía `GetRequiredService<IChatCompletionService>()`. Se implementa `ChatHistory` con tres roles: System (comportamiento del LLM), User (mensajes del usuario) y Assistant (respuestas del LLM). Se demuestra cómo el historial preserva contexto entre turnos. Se introduce el chat multi-modal para procesar imágenes con `ImageContent`.

### SECCIÓN 5: Workshop — Creando una aplicación de Chat Multi-modal (5 lecciones — 33 min)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 27 | Introducción a la Sección | Video | 1:34 |
| 28 | Creando y configurando el proyecto | Video | 4:29 |
| 29 | Desplegando las instrucciones del chat | Video | 3:05 |
| 30 | Interactuando con el servicio de chat | Video | 7:10 |
| 31 | Agregando la habilidad de leer imágenes | Video | 16:27 |

**Resumen:** Proyecto de chat interactivo que soporta texto e imágenes. Se implementa un loop de chat con comandos (`img` para imágenes, `exit` para salir). Se crea `ChatMessageContentItemCollection` para enviar contenido multimodal (texto + imágenes). Se soportan URLs e imágenes locales con inferencia de MIME type. Se usa Spectre.Console para validación de inputs y formateo de consola.

### SECCIÓN 6: Plugins (13 lecciones — 1h 32m)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 32 | Los fundamentos de los plugins en Semantic Kernel | Video | 1:33 |
| 33 | Creando Kernel Functions | Video | 8:56 |
| 34 | Creando Plugins nativos | Video | 1:48 |
| 35 | Creando tu primer plugin nativo | Video | 10:41 |
| 36 | Usando plugins pre-creados | Video | 8:04 |
| 37 | Function Calling | Video | 3:00 |
| 38 | Function Calling en Semantic Kernel | Video | 6:34 |
| 39 | Function Calling en acción | Video | 13:17 |
| 40 | AddFromObject vs AddFromType | Video | 9:12 |
| 41 | Function Choice Behavior | Video | 7:04 |
| 42 | Function Invocation | Video | 6:09 |
| 43 | Lidiando con objetos complejos como parámetros | Video | 5:06 |
| 44 | Agregando plugins OpenAPI | Video | 10:50 |

**Resumen:** Los plugins son el componente fundamental de Semantic Kernel. Se crean funciones con `[KernelFunction]` y `[Description]` para que el LLM las "vea" y decida cuándo invocarlas. Se implementa Function Calling automático con `FunctionChoiceBehavior.Auto()`. Se diferencia `AddFromObject` (inyectar instancia con dependencias) vs `AddFromType` (crear instancia sin constructor params). Se implementa `FunctionInvocationFilter` para interceptar invocaciones. Se agregan plugins OpenAPI desde especificaciones externas.

### SECCIÓN 7: Workshop — Creando una aplicación que obtiene datos de un video (8 lecciones — 1h 07m)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 45 | Introducción a la Sección | Video | 1:42 |
| 46 | Creando y configurando el proyecto inicial | Video | 10:07 |
| 47 | Instalando FFmpeg | Video | 4:07 |
| 48 | Extrayendo el audio de un archivo de video | Video | 9:37 |
| 49 | Comprimiendo el archivo de audio para la transcripción | Video | 7:48 |
| 50 | Implementando Speech to Text para obtener la transcripción | Video | 14:15 |
| 51 | Cortando momentos del video | Video | 8:50 |
| 52 | Quemando subtítulos | Video | 10:52 |

**Resumen:** Proyecto que extrae información de videos usando FFmpeg y Semantic Kernel. Se extrae audio de video, se comprime para respetar límite de 25MB de Whisper, se transcribe con Speech-to-Text, se generan clips de momentos destacados y se queman subtítulos SRT en el video. Se crea un plugin `VideoPlugin` con funciones para cada operación de FFmpeg.

### SECCIÓN 8: Prompting (7 lecciones — 44 min)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 53 | Fundamentos del Prompting | Video | 12:50 |
| 54 | Usando prompt templates en Semantic Kernel | Video | 3:42 |
| 55 | Convirtiendo prompts en instancias ChatHistory | Video | 2:54 |
| 56 | Usando variables en prompt templates | Video | 3:13 |
| 57 | Prompt templates Handlebars | Video | 7:00 |
| 58 | Prompt templates Liquid | Video | 3:15 |
| 59 | Separando prompt templates en archivos YAML | Video | 10:50 |

**Resumen:** Se cubren técnicas de prompting: Zero-Shot, One-Shot, Few-Shot, Chain of Thought y Generated Knowledge. Se implementan tres formatos de plantillas: SK nativo (`{{$variable}}`), Handlebars (`{{variable}}`) y Liquid (`{{variable}}`). Se separan prompts en archivos YAML con configuración de `execution_settings`, `input_variables` y template embebido. Se usa `CreateFunctionFromPrompt` con `PromptTemplateConfig`.

### SECCIÓN 9: Workshop — Creando una aplicación generadora de podcasts (8 lecciones — 1h 02m)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 60 | Introducción a la sección | Video | 1:20 |
| 61 | Configurando el proyecto | Video | 2:00 |
| 62 | Recolección de datos de entrada | Video | 8:59 |
| 63 | Conversión markdown | Video | 9:26 |
| 64 | Generando la lluvia de ideas | Video | 11:41 |
| 65 | Generando el script | Video | 8:41 |
| 66 | Generación de la conclusión | Video | 4:40 |
| 67 | Generación del podcast | Video | 15:05 |

**Resumen:** Proyecto que genera podcasts completos a partir de URLs o texto. Se recolectan datos de entrada (URLs, temas, formato), se convierte contenido HTML a Markdown con la librería ReverseMarkdown, se genera lluvia de ideas con el LLM, se crea el script del podcast, se genera la conclusión y finalmente se sintetiza el audio completo con TTS de Azure OpenAI. Todo el flujo usa plantillas YAML separadas.

### SECCIÓN 10: Memoria (Vector Stores) y Búsqueda por Texto (8 lecciones — 1h 10m)

| # | Lección | Tipo | Duración |
|---|---------|------|----------|
| 68 | ¿Qué son los Embeddings y Vector Stores? | Video | 4:02 |
| 69 | Definiendo el modelo de datos | Video | 8:48 |
| 70 | Generando embeddings y guardándolos en Vector Stores | Video | 13:19 |
| 71 | Llevando a cabo búsqueda vectorial | Video | 9:56 |
| 72 | Búsqueda por Texto | Video | 9:25 |
| 73 | Plugins de búsqueda por texto | Video | 9:43 |
| 74 | Plugins de búsqueda por texto — Function Calling | Video | 8:51 |
| 75 | Búsqueda por texto con Vector Stores | Video | 6:15 |

**Resumen:** Se introducen embeddings como representaciones vectoriales de texto. Se define un modelo de datos con atributos `[VectorStoreRecordKey]`, `[VectorStoreRecordData]` y `[VectorStoreRecordVector]`. Se generan embeddings con `TextEmbeddingGenerationService` (modelo text-embedding-ada-002) y se almacenan en un Vector Store en memoria (`InMemoryVectorStore`). Se implementa búsqueda vectorial y búsqueda por texto (`VectorStoreTextSearch`). Se crean plugins de búsqueda que se integran con Function Calling para RAG (Retrieval-Augmented Generation).

---

## Jornalización Detallada — 10 Sesiones (30 horas)

---

### SEMANA 1

---

#### 📅 Sesión 1 (Lunes) — Fundamentos y El Kernel Core

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Introducción a Semantic Kernel + Configuración del Kernel |
| **Secciones Udemy** | Sección 1 completa (4 lecciones) + Sección 2 lecciones 5-10 |
| **Subtemas** | Qué es SK, componentes, beneficios, patrón Builder, API Keys (OpenAI/Azure), variables de entorno, creación de Kernels |
| **Herramientas** | Visual Studio 2022, NuGet (`Microsoft.SemanticKernel`, `Swashbuckle.AspNetCore`), OpenAI API, Azure OpenAI Service |
| **Duración** | 3 horas |

**Contenido Teórico (1h):**
- Qué es Semantic Kernel: SDK de código abierto de Microsoft para integrar LLMs en aplicaciones
- Arquitectura: Kernel → Services → Plugins → Planners → Memory
- Componentes: AI Services, Plugins, Prompt Templates, Filters
- El patrón Builder: `Kernel.CreateBuilder()` → configurar servicios → `.Build()`
- OpenAI vs Azure OpenAI: diferencias en autenticación y configuración
- Inyección del Kernel como servicio en ASP.NET Core
- `appsettings.json` para gestión segura de API Keys (nunca en código fuente)

**Actividad Práctica (2h):**

> **Ejercicio 1.1 — Crear el proyecto Web API + Swagger + Semantic Kernel**
> Crear un proyecto ASP.NET Core Web API, instalar paquetes, configurar el Kernel en `Program.cs` con inyección de dependencias y crear los primeros endpoints.

```bash
# Crear el proyecto Web API
dotnet new webapi -n CursoSK.Api -controllers
cd CursoSK.Api
dotnet add package Microsoft.SemanticKernel
dotnet add package Swashbuckle.AspNetCore
```

```json
// appsettings.json — Configuración de LLM (NO subir a repositorio público)
{
  "LLMSettings": {
    "Provider": "azure",
    "OpenAI": {
      "ModelId": "gpt-4o-mini",
      "ApiKey": "sk-..."
    },
    "AzureOpenAI": {
      "DeploymentName": "gpt-4o-mini",
      "Endpoint": "https://tu-recurso.openai.azure.com",
      "ApiKey": "..."
    }
  }
}
```

```csharp
// Program.cs — Inyección del Kernel en el pipeline de ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Semantic Kernel como servicio singleton
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

```csharp
// Controllers/KernelController.cs — Primeros endpoints
[ApiController]
[Route("api/[controller]")]
public class KernelController : ControllerBase
{
    private readonly Kernel _kernel;
    public KernelController(Kernel kernel) => _kernel = kernel;

    /// <summary>Genera texto a partir de un prompt libre.</summary>
    [HttpPost("prompt")]
    public async Task<IActionResult> InvokePrompt([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { response = result.ToString() });
    }

    /// <summary>Genera texto con parámetros de ejecución configurables.</summary>
    [HttpPost("prompt/configurado")]
    public async Task<IActionResult> InvokePromptConSettings([FromBody] PromptConSettingsRequest request)
    {
        var settings = new OpenAIPromptExecutionSettings
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(settings));
        return Ok(new { response = result.ToString() });
    }
}

// DTOs
public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
```

> **Ejercicio 1.2 — Probar desde Swagger**
> Ejecutar el proyecto (`dotnet run`), abrir `https://localhost:xxxx/swagger` y probar los endpoints:
> - `POST /api/kernel/prompt` → `{ "prompt": "Escribe un poema corto sobre Semantic Kernel" }`
> - `POST /api/kernel/prompt/configurado` → `{ "prompt": "Genera 3 ideas de negocio con IA", "maxTokens": 100, "temperature": 0.9 }`

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo se configura el Kernel en el sistema de producción del laboratorio clínico con soporte multi-LLM:
```csharp
// Program.cs — Kernel con Azure OpenAI + selección dinámica de proveedor
builder.Services.AddSingleton<Kernel>(sp => {
    var config = sp.GetRequiredService<IConfiguration>();
    var llmProvider = config["LLMSettings:Provider"]?.ToLower() ?? "chatgpt";
    var kernelBuilder = Kernel.CreateBuilder();
    
    if (llmProvider == "gemini") {
        kernelBuilder.AddGoogleAIGeminiChatCompletion(
            modelId: config["LLMSettings:Gemini:ModelId"],
            apiKey: config["IAkey:keygemini"]);
    } else {
        kernelBuilder.AddAzureOpenAIChatCompletion(
            deploymentName: config["LLMSettings:ChatGPT:DeploymentName"],
            endpoint: config["LLMSettings:ChatGPT:Endpoint"],
            apiKey: config["IAkey:keygpt"]);
    }
    return kernelBuilder.Build();
});
```

---

#### 📅 Sesión 2 (Miércoles) — Servicios Multimodales del Kernel

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Chat Completion, Streaming, Generación de Imágenes, Audio TTS y Whisper |
| **Secciones Udemy** | Sección 2 lecciones 11-16 |
| **Subtemas** | Chat Completion, Streaming, DALL-E, Text-to-Speech, Audio-to-Text (Whisper), OpenAIPromptExecutionSettings |
| **Herramientas** | ASP.NET Core Web API, Swagger, OpenAI API (DALL-E 3, TTS, Whisper), Azure OpenAI, `#pragma warning disable SKEXP0010` |
| **Duración** | 3 horas |

**Contenido Teórico (45min):**
- Chat Completion: `InvokePromptAsync()` y `GetChatMessageContentAsync()`
- Streaming: `GetStreamingChatMessageContentsAsync()` — respuesta token por token vía SSE
- DALL-E: `AddOpenAITextToImage()`, `ITextToImageService`, `OpenAITextToImageExecutionSettings`
- Text-to-Speech: `AddOpenAITextToAudio()`, modelos de voz (alloy, echo, fable, etc.)
- Whisper (Audio-to-Text): `AddOpenAIAudioToText()`, límite de 25MB
- Pragma experimental: `#pragma warning disable SKEXP0010`
- Registrar servicios multimodales en `Program.cs`

**Actividad Práctica (2h 15m):**

> **Ejercicio 2.1 — Agregar servicios multimodales al Kernel y endpoint de Streaming**

```csharp
// Program.cs — Agregar servicios de imagen, audio y texto-a-audio al Kernel
#pragma warning disable SKEXP0010
kernelBuilder.AddOpenAITextToImage(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!,
    modelId: "dall-e-3");
kernelBuilder.AddOpenAITextToAudio(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!,
    modelId: "tts-1");
kernelBuilder.AddOpenAIAudioToText(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!,
    modelId: "whisper-1");
#pragma warning restore SKEXP0010
```

```csharp
// Controllers/MultimodalController.cs
[ApiController]
[Route("api/[controller]")]
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
}
```

> **Ejercicio 2.2 — Endpoint de Generación de Imágenes con DALL-E**

```csharp
/// <summary>Genera una imagen con DALL-E a partir de un prompt.</summary>
[HttpPost("imagen")]
public async Task<IActionResult> GenerarImagen([FromBody] ImagenRequest request)
{
    #pragma warning disable SKEXP0010
    var imageService = _kernel.GetRequiredService<ITextToImageService>();
    var images = await imageService.GetImageContentsAsync(
        request.Descripcion,
        new OpenAITextToImageExecutionSettings
        {
            Size = (1792, 1024),
            Quality = request.Quality ?? "hd",
            Style = request.Style ?? "vivid"
        });
    return Ok(new { url = images[0].Uri!.ToString() });
    #pragma warning restore SKEXP0010
}

public record ImagenRequest(string Descripcion, string? Quality = "hd", string? Style = "vivid");
```

> **Ejercicio 2.3 — Endpoints de Text-to-Speech y Whisper**

```csharp
/// <summary>Convierte texto a audio MP3 (Text-to-Speech).</summary>
[HttpPost("tts")]
public async Task<IActionResult> TextToSpeech([FromBody] TTSRequest request)
{
    #pragma warning disable SKEXP0010
    var audioService = _kernel.GetRequiredService<ITextToAudioService>();
    var audioContent = await audioService.GetAudioContentAsync(
        request.Texto,
        new OpenAITextToAudioExecutionSettings("tts-1") { Voice = request.Voice ?? "alloy" });
    return File(audioContent.Data!.ToArray(), "audio/mpeg", "audio.mp3");
    #pragma warning restore SKEXP0010
}

/// <summary>Transcribe un archivo de audio a texto (Whisper STT).</summary>
[HttpPost("stt")]
public async Task<IActionResult> SpeechToText(IFormFile archivo)
{
    #pragma warning disable SKEXP0010
    using var ms = new MemoryStream();
    await archivo.CopyToAsync(ms);
    var whisperService = _kernel.GetRequiredService<IAudioToTextService>();
    var transcription = await whisperService.GetTextContentAsync(
        new AudioContent(ms.ToArray(), archivo.ContentType));
    return Ok(new { texto = transcription.Text });
    #pragma warning restore SKEXP0010
}

public record TTSRequest(string Texto, string? Voice = "alloy");
```

> **Probar desde Swagger:**
> - `POST /api/multimodal/stream` → streaming de texto
> - `POST /api/multimodal/imagen` → `{ "descripcion": "Un paisaje futurista" }`
> - `POST /api/multimodal/tts` → `{ "texto": "Bienvenidos al curso" }`
> - `POST /api/multimodal/stt` → subir archivo de audio desde Swagger

**🔗 Conexión con el Proyecto Real:**
Se muestra la transcripción de notas de voz de WhatsApp en producción:
```csharp
// CotizacionIntegration.cs — TranscribeAudio()
// Descarga el audio OGG de WhatsApp, lo envía a Azure OpenAI gpt-4o-transcribe
var transcriptionUrl = $"{azureEndpoint}/openai/deployments/{deploymentName}/audio/transcriptions?api-version=2025-01-01-preview";
formContent.Add(audioContent, "file", "audio.ogg");
formContent.Add(new StringContent("es"), "language");
```

---

#### 📅 Sesión 3 (Viernes) — Workshop: Generador de Blog Posts para WordPress

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Proyecto Completo #1: Generador de Blog Posts con SK |
| **Secciones Udemy** | Sección 3 completa (7 lecciones) |
| **Subtemas** | Prompts para bloques Gutenberg, generación de imágenes destacadas, TTS, publicación en WordPress REST API |
| **Herramientas** | Semantic Kernel, ASP.NET Core Web API, Swagger, DALL-E 3, TTS, WordPress REST API |
| **Duración** | 3 horas |

**Contenido Teórico (30min):**
- Bloques Gutenberg de WordPress: estructura HTML con comentarios `<!-- wp:heading -->` etc.
- Diseño de prompts que generan HTML estructurado
- Patrón Service Layer: lógica de negocio en servicios inyectados, no en controladores
- WordPress REST API: autenticación, crear posts con contenido HTML

**Actividad Práctica — Proyecto Completo (2h 30m):**

> **Proyecto 1: Blog Post Generator API**
> Agregar un controlador `BlogController` a la Web API con endpoints para:
> 1. Generar contenido HTML del blog con bloques Gutenberg
> 2. Generar imagen destacada con DALL-E
> 3. Generar audio TTS del contenido
> 4. Publicar automáticamente en WordPress

```csharp
// Services/BlogService.cs — Servicio de generación de blog
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

    public async Task<string> GenerarImagenDestacada(string tema)
    {
        #pragma warning disable SKEXP0010
        var imageService = _kernel.GetRequiredService<ITextToImageService>();
        var images = await imageService.GetImageContentsAsync(
            $"Imagen destacada profesional para un blog post sobre: {tema}",
            new OpenAITextToImageExecutionSettings { Size = (1792, 1024), Quality = "hd" });
        return images[0].Uri!.ToString();
        #pragma warning restore SKEXP0010
    }
}

// Registrar en Program.cs:
// builder.Services.AddSingleton<BlogService>();
```

```csharp
// Controllers/BlogController.cs
[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly BlogService _blogService;
    private readonly Kernel _kernel;

    public BlogController(BlogService blogService, Kernel kernel)
    {
        _blogService = blogService;
        _kernel = kernel;
    }

    /// <summary>Genera contenido HTML con bloques Gutenberg para un blog post.</summary>
    [HttpPost("generar")]
    public async Task<IActionResult> GenerarBlogPost([FromBody] BlogRequest request)
    {
        var contenido = await _blogService.GenerarContenidoBlog(request.Tema);
        var imagenUrl = await _blogService.GenerarImagenDestacada(request.Tema);
        return Ok(new { contenidoHtml = contenido, imagenDestacada = imagenUrl });
    }

    /// <summary>Genera blog post completo y lo publica en WordPress.</summary>
    [HttpPost("publicar")]
    public async Task<IActionResult> PublicarEnWordPress([FromBody] BlogPublicarRequest request)
    {
        var contenido = await _blogService.GenerarContenidoBlog(request.Tema);
        var imagenUrl = await _blogService.GenerarImagenDestacada(request.Tema);

        // Publicar en WordPress vía REST API
        using var http = new HttpClient();
        var authHeader = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes($"{request.WpUser}:{request.WpAppPassword}"));
        http.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);
        
        var wpResponse = await http.PostAsJsonAsync($"{request.WpUrl}/wp-json/wp/v2/posts", new
        {
            title = request.Tema,
            content = contenido,
            status = "draft"
        });
        
        var wpResult = await wpResponse.Content.ReadAsStringAsync();
        return Ok(new { wordpress = wpResult, imagenDestacada = imagenUrl });
    }
}

public record BlogRequest(string Tema);
public record BlogPublicarRequest(string Tema, string WpUrl, string WpUser, string WpAppPassword);
```

> **Probar desde Swagger:**
> - `POST /api/blog/generar` → `{ "tema": "Cómo usar Semantic Kernel en .NET" }`
> - `POST /api/blog/publicar` → incluir credenciales de WordPress

---

### SEMANA 2

---

#### 📅 Sesión 4 (Lunes) — Chat History + Workshop Chat Multi-modal

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Chat Completion avanzado + Proyecto de Chat Multi-modal |
| **Secciones Udemy** | Sección 4 completa (3 lecciones) + Sección 5 completa (5 lecciones) |
| **Subtemas** | IChatCompletionService, ChatHistory (System/User/Assistant), Chat multi-modal con imágenes, bucle interactivo de chat |
| **Herramientas** | Semantic Kernel, ASP.NET Core Web API, Swagger, ChatHistory, ImageContent |
| **Duración** | 3 horas |

**Contenido Teórico (45min):**
- `IChatCompletionService`: acceso directo al servicio de chat
- `ChatHistory`: tres roles — `AddSystemMessage()`, `AddUserMessage()`, `AddAssistantMessage()`
- Preservación de contexto entre turnos de conversación
- Chat multi-modal: `ChatMessageContentItemCollection` para texto + imágenes
- `ImageContent` con URL o bytes locales + MIME type
- Gestión de sesiones de chat en una API (ConcurrentDictionary por sessionId)

**Actividad Práctica (2h 15m):**

> **Ejercicio 4.1 — Endpoint de Chat con Historia por Sesión**

```csharp
// Services/ChatSessionService.cs — Servicio de gestión de sesiones de chat
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
}

// Registrar en Program.cs:
// builder.Services.AddSingleton<ChatSessionService>();
```

```csharp
// Controllers/ChatController.cs
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatSessionService _chatService;
    private readonly Kernel _kernel;

    public ChatController(ChatSessionService chatService, Kernel kernel)
    {
        _chatService = chatService;
        _kernel = kernel;
    }

    /// <summary>Envía un mensaje de texto a una sesión de chat (mantiene historial).</summary>
    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
    {
        var respuesta = await _chatService.EnviarMensaje(sessionId, request.Mensaje);
        return Ok(new { sessionId, respuesta });
    }

    /// <summary>Envía una imagen (URL) al chat para análisis multimodal.</summary>
    [HttpPost("{sessionId}/imagen")]
    public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
    {
        var history = _chatService.ObtenerHistorial(sessionId);
        if (history == null) return NotFound("Sesión no encontrada. Envíe un mensaje primero.");

        var contents = new ChatMessageContentItemCollection();
        contents.Add(new TextContent(request.Pregunta ?? "Describe esta imagen en detalle"));
        contents.Add(new ImageContent(new Uri(request.ImagenUrl)));
        history.AddUserMessage(contents);

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(history);
        history.AddAssistantMessage(response.Content!);

        return Ok(new { sessionId, respuesta = response.Content });
    }

    /// <summary>Envía una imagen (archivo local) al chat para análisis multimodal.</summary>
    [HttpPost("{sessionId}/imagen/upload")]
    public async Task<IActionResult> UploadImagen(string sessionId, IFormFile imagen, [FromForm] string? pregunta)
    {
        var history = _chatService.ObtenerHistorial(sessionId);
        if (history == null) return NotFound("Sesión no encontrada. Envíe un mensaje primero.");

        using var ms = new MemoryStream();
        await imagen.CopyToAsync(ms);
        var mimeType = imagen.ContentType ?? "image/jpeg";

        var contents = new ChatMessageContentItemCollection();
        contents.Add(new TextContent(pregunta ?? "Describe esta imagen en detalle"));
        contents.Add(new ImageContent(ms.ToArray(), mimeType));
        history.AddUserMessage(contents);

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(history);
        history.AddAssistantMessage(response.Content!);

        return Ok(new { sessionId, respuesta = response.Content });
    }

    /// <summary>Obtiene el historial completo de una sesión.</summary>
    [HttpGet("{sessionId}/historial")]
    public IActionResult ObtenerHistorial(string sessionId)
    {
        var history = _chatService.ObtenerHistorial(sessionId);
        if (history == null) return NotFound("Sesión no encontrada");
        return Ok(history.Select(m => new { rol = m.Role.Label, contenido = m.Content }));
    }

    /// <summary>Elimina una sesión de chat.</summary>
    [HttpDelete("{sessionId}")]
    public IActionResult EliminarSesion(string sessionId)
    {
        _chatService.EliminarSesion(sessionId);
        return Ok(new { mensaje = "Sesión eliminada" });
    }
}

public record ChatMensajeRequest(string Mensaje);
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);
```

> **Probar desde Swagger — Flujo de conversación:**
> 1. `POST /api/chat/session1/mensaje` → `{ "mensaje": "Mi nombre es José" }`
> 2. `POST /api/chat/session1/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }` → ¡Recuerda!
> 3. `POST /api/chat/session1/imagen` → `{ "imagenUrl": "https://...", "pregunta": "¿Qué ves?" }`
> 4. `GET /api/chat/session1/historial` → ver toda la conversación

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo el agente del laboratorio mantiene estado por sesión de WhatsApp:
```csharp
// CotizacionIntegration.cs — Estado conversacional por teléfono
private readonly Dictionary<string, ConversationSession> _activeSessions;
// Cada ConversationSession tiene: PhoneNumber, CurrentIntent, CurrentState, ChatHistory
```

---

#### 📅 Sesión 5 (Miércoles) — Fundamentos de Plugins y Kernel Functions

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Plugins nativos: Kernel Functions, Plugins y Function Calling |
| **Secciones Udemy** | Sección 6 lecciones 32-39 |
| **Subtemas** | `[KernelFunction]`, `[Description]`, plugins nativos, plugins pre-construidos, Function Calling automático |
| **Herramientas** | Semantic Kernel Plugins, Auto Function Calling, ASP.NET Core Web API, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (1h):**
- Plugins: unidades de funcionalidad que el Kernel puede invocar
- `[KernelFunction("nombre")]`: registra un método como función invocable por el LLM
- `[Description("...")]`: describe la función para que el LLM sepa cuándo usarla
- Registro: `kernel.Plugins.AddFromObject(instance)` o `kernel.Plugins.AddFromType<T>()`
- Function Calling: el LLM decide qué funciones invocar basándose en las descripciones
- `FunctionChoiceBehavior.Auto()`: invocación automática de funciones
- Plugins pre-construidos: `Microsoft.SemanticKernel.Plugins.Core` (TimePlugin, etc.)

**Actividad Práctica (2h):**

> **Ejercicio 5.1 — Crear un Plugin Nativo y exponerlo vía API**

```csharp
// Plugins/ClimaPlugin.cs
public class ClimaPlugin
{
    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad específica")]
    public string ObtenerClima(
        [Description("Nombre de la ciudad")] string ciudad)
    {
        var climas = new Dictionary<string, string>
        {
            { "tegucigalpa", "☀️ 28°C, Soleado" },
            { "san pedro sula", "🌤️ 32°C, Parcialmente nublado" },
            { "la ceiba", "🌧️ 26°C, Lluvioso" }
        };
        return climas.GetValueOrDefault(ciudad.ToLower(), $"No tengo datos para {ciudad}");
    }

    [KernelFunction("obtener_fecha_hora")]
    [Description("Obtiene la fecha y hora actual")]
    public string ObtenerFechaHora()
    {
        return $"Fecha: {DateTime.Now:dd/MM/yyyy}, Hora: {DateTime.Now:hh:mm tt}";
    }
}
```

```csharp
// Plugins/MathPlugin.cs
public class MathPlugin
{
    [KernelFunction("sumar")]
    [Description("Suma dos números")]
    public double Sumar(
        [Description("Primer número")] double a,
        [Description("Segundo número")] double b) => a + b;

    [KernelFunction("multiplicar")]
    [Description("Multiplica dos números")]
    public double Multiplicar(
        [Description("Primer número")] double a,
        [Description("Segundo número")] double b) => a * b;
}
```

```csharp
// Program.cs — Registrar plugins en el Kernel (después de Build)
var kernel = kernelBuilder.Build();
kernel.Plugins.AddFromObject(new ClimaPlugin());
kernel.Plugins.AddFromObject(new MathPlugin());
builder.Services.AddSingleton(kernel);
```

```csharp
// Controllers/AgentController.cs — Endpoint con Function Calling automático
[ApiController]
[Route("api/[controller]")]
public class AgentController : ControllerBase
{
    private readonly Kernel _kernel;
    public AgentController(Kernel kernel) => _kernel = kernel;

    /// <summary>
    /// Envía un prompt al agente con Function Calling automático.
    /// El LLM decide qué plugins invocar según la consulta.
    /// </summary>
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

    /// <summary>Lista todos los plugins y funciones registrados en el Kernel.</summary>
    [HttpGet("plugins")]
    public IActionResult ListarPlugins()
    {
        var plugins = _kernel.Plugins.Select(p => new
        {
            nombre = p.Name,
            funciones = p.Select(f => new { nombre = f.Name, descripcion = f.Description })
        });
        return Ok(plugins);
    }
}
```

> **Probar desde Swagger:**
> - `GET /api/agent/plugins` → ver todos los plugins y funciones registrados
> - `POST /api/agent/consultar` → `{ "prompt": "¿Qué clima hace en Tegucigalpa y qué hora es?" }`
>   - El LLM invocará automáticamente `obtener_clima("tegucigalpa")` y `obtener_fecha_hora()`
> - `POST /api/agent/consultar` → `{ "prompt": "¿Cuánto es (15 + 27) * 3?" }`
>   - Invocará `sumar(15, 27)` → 42, luego `multiplicar(42, 3)` → 126

**🔗 Conexión con el Proyecto Real:**
Se estudian los plugins del laboratorio clínico como ejemplo de producción:
```csharp
// CotizacionPlugin.cs — Plugin con inyección de dependencias
public class CotizacionPlugin
{
    private readonly DbContextFactory _contextFactory;
    private readonly IHttpClientFactory _httpClientFactory;

    [KernelFunction("buscar_cliente_identidad")]
    [Description("Busca un cliente por su número de identidad")]
    public async Task<dynamic> GetMgCliente(
        [Description("Número de identidad sin guiones")] string NumeroIdentidad) { ... }

    [KernelFunction("guardar_cotizacion")]
    [Description("Guarda una cotización en la base de datos")]
    public async Task<string> GuardarCotizacion(...) { ... }
}

// Registro con AddFromObject (necesario cuando el plugin tiene constructor con parámetros)
kernel.Plugins.AddFromObject(new CotizacionPlugin(dbContextFactory, httpClientFactory, config));
```

---

#### 📅 Sesión 6 (Viernes) — Plugins Avanzados: AddFromObject, Filters y OpenAPI

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Plugins avanzados: AddFromObject vs AddFromType, Function Choice Behavior, Invocation Filters, OpenAPI |
| **Secciones Udemy** | Sección 6 lecciones 40-44 |
| **Subtemas** | Inyección de dependencias en plugins, FunctionChoiceBehavior, FunctionInvocationFilter, objetos complejos, plugins OpenAPI |
| **Herramientas** | Semantic Kernel (Plugins, Filters), OpenAPI, ASP.NET Core DI, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (45min):**
- `AddFromObject(instance)`: para plugins con constructor/dependencias (DbContext, HttpClient, etc.)
- `AddFromType<T>()`: para plugins con constructor sin parámetros
- `FunctionChoiceBehavior`: `Auto()`, `Required()`, `None()`
- `IFunctionInvocationFilter`: interceptar antes/después de cada invocación de función
- Objetos complejos como parámetros de función
- Plugins OpenAPI: importar APIs externas desde archivos de especificación

**Actividad Práctica (2h 15m):**

> **Ejercicio 6.1 — Plugin con HttpClient inyectado vía DI**

```csharp
// Plugins/WikipediaPlugin.cs — Plugin con dependencia (HttpClient)
public class WikipediaPlugin
{
    private readonly HttpClient _httpClient;
    
    public WikipediaPlugin(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [KernelFunction("buscar_en_wikipedia")]
    [Description("Busca información en Wikipedia sobre un tema")]
    public async Task<string> BuscarEnWikipedia(
        [Description("Tema a buscar")] string tema)
    {
        var url = $"https://es.wikipedia.org/api/rest_v1/page/summary/{Uri.EscapeDataString(tema)}";
        var response = await _httpClient.GetStringAsync(url);
        var json = JsonSerializer.Deserialize<JsonElement>(response);
        return json.GetProperty("extract").GetString() ?? "No encontrado";
    }
}

// Program.cs — Registrar con AddFromObject (porque necesita HttpClient del DI)
kernel.Plugins.AddFromObject(new WikipediaPlugin(new HttpClient()));
```

> **Ejercicio 6.2 — Function Invocation Filter como Middleware de Logging**

```csharp
// Filters/LoggingFilter.cs
public class LoggingFilter : IFunctionInvocationFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger) => _logger = logger;

    public async Task OnFunctionInvocationAsync(
        FunctionInvocationContext context, 
        Func<FunctionInvocationContext, Task> next)
    {
        _logger.LogInformation("🔧 Invocando: {Function} con args: {Args}", 
            context.Function.Name, JsonSerializer.Serialize(context.Arguments));
        
        await next(context);
        
        _logger.LogInformation("✅ Resultado de {Function}: {Result}", 
            context.Function.Name, context.Result);
    }
}

// Program.cs — Registrar el filtro en el Kernel
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter>(sp =>
    new LoggingFilter(sp.GetRequiredService<ILogger<LoggingFilter>>()));
```

> **Ejercicio 6.3 — Plugin OpenAPI**

```csharp
// Controllers/AgentController.cs — Endpoint para cargar plugin OpenAPI dinámicamente
/// <summary>Importa un plugin desde una especificación OpenAPI.</summary>
[HttpPost("plugins/openapi")]
public async Task<IActionResult> CargarPluginOpenApi([FromBody] OpenApiPluginRequest request)
{
    #pragma warning disable SKEXP0040
    await _kernel.ImportPluginFromOpenApiAsync(request.Nombre, 
        new Uri(request.SpecUrl),
        new OpenApiFunctionExecutionParameters
        {
            EnablePayloadNamespacing = true
        });
    #pragma warning restore SKEXP0040
    return Ok(new { mensaje = $"Plugin '{request.Nombre}' cargado exitosamente" });
}

public record OpenApiPluginRequest(string Nombre, string SpecUrl);
```

> **Probar desde Swagger:**
> - `POST /api/agent/consultar` → `{ "prompt": "Busca en Wikipedia qué es Semantic Kernel" }`
> - `POST /api/agent/plugins/openapi` → `{ "nombre": "PetStore", "specUrl": "https://petstore3.swagger.io/api/v3/openapi.json" }`
> - Los logs del servidor muestran cada Function Calling interceptado por el filtro

**🔗 Conexión con el Proyecto Real:**
Se muestra el patrón `AddFromObject` en producción para los 6 plugins del laboratorio:
```csharp
// CotizacionIntegration.cs — Registro de plugins con dependencias
var dbContextFactory = serviceProvider.GetRequiredService<DbContextFactory>();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

kernel.Plugins.AddFromObject(new CotizacionPlugin(dbContextFactory, httpClientFactory, configuration));
kernel.Plugins.AddFromObject(new CitaPlugin(dbContextFactory));
kernel.Plugins.AddFromObject(new DomicilioPlugin(dbContextFactory, httpClientFactory, configuration));
kernel.Plugins.AddFromObject(new FechaHoraPlugin()); // Este sí podría ser AddFromType
```

---

### SEMANA 3

---

#### 📅 Sesión 7 (Lunes) — Workshop: Extracción de Datos de Video

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Proyecto Completo #3: Análisis de Video con FFmpeg + Semantic Kernel |
| **Secciones Udemy** | Sección 7 completa (8 lecciones) |
| **Subtemas** | FFmpeg, extracción de audio, compresión, transcripción Whisper, corte de clips, quemado de subtítulos SRT |
| **Herramientas** | FFmpeg, Semantic Kernel (Whisper), CliWrap, ASP.NET Core Web API, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (30min):**
- FFmpeg: herramienta de línea de comandos para procesamiento multimedia
- Flujo del proyecto: Video → Extraer audio → Comprimir → Transcribir → Cortar clips → Subtítulos
- CliWrap: librería .NET para ejecutar procesos externos de forma segura
- Límite de Whisper: 25MB máximo por archivo de audio
- Formato SRT para subtítulos
- Manejo de archivos temporales en una API (IWebHostEnvironment, Path.GetTempPath)

**Actividad Práctica — Proyecto Completo (2h 30m):**

> **Proyecto 3: Video Data Extractor API**
> Agregar un controlador `VideoController` con endpoints para cada operación sobre video.

```csharp
// Plugins/VideoPlugin.cs — Plugin para FFmpeg (se registra en el Kernel para Function Calling)
public class VideoPlugin
{
    [KernelFunction("extraer_audio")]
    [Description("Extrae el audio de un archivo de video y lo guarda como MP3")]
    public async Task<string> ExtraerAudio(
        [Description("Ruta del archivo de video")] string videoPath,
        [Description("Ruta de salida del audio")] string outputPath)
    {
        await Cli.Wrap("ffmpeg")
            .WithArguments($"-i \"{videoPath}\" -vn -acodec libmp3lame \"{outputPath}\" -y")
            .ExecuteAsync();
        return $"Audio extraído en: {outputPath}";
    }

    [KernelFunction("comprimir_audio")]
    [Description("Comprime un archivo de audio para que sea menor a 25MB")]
    public async Task<string> ComprimirAudio(
        [Description("Ruta del archivo de audio")] string audioPath,
        [Description("Ruta de salida comprimida")] string outputPath)
    {
        await Cli.Wrap("ffmpeg")
            .WithArguments($"-i \"{audioPath}\" -b:a 32k -ar 16000 -ac 1 \"{outputPath}\" -y")
            .ExecuteAsync();
        return $"Audio comprimido en: {outputPath}";
    }

    [KernelFunction("cortar_clip")]
    [Description("Corta un segmento específico de un video")]
    public async Task<string> CortarClip(
        [Description("Ruta del video original")] string videoPath,
        [Description("Tiempo de inicio (HH:MM:SS)")] string inicio,
        [Description("Duración del clip (HH:MM:SS)")] string duracion,
        [Description("Ruta de salida del clip")] string outputPath)
    {
        await Cli.Wrap("ffmpeg")
            .WithArguments($"-i \"{videoPath}\" -ss {inicio} -t {duracion} -c copy \"{outputPath}\" -y")
            .ExecuteAsync();
        return $"Clip generado en: {outputPath}";
    }

    [KernelFunction("quemar_subtitulos")]
    [Description("Quema subtítulos SRT en un archivo de video")]
    public async Task<string> QuemarSubtitulos(
        [Description("Ruta del video")] string videoPath,
        [Description("Ruta del archivo SRT")] string srtPath,
        [Description("Ruta de salida")] string outputPath)
    {
        await Cli.Wrap("ffmpeg")
            .WithArguments($"-i \"{videoPath}\" -vf subtitles=\"{srtPath}\" \"{outputPath}\" -y")
            .ExecuteAsync();
        return $"Video con subtítulos en: {outputPath}";
    }
}
```

```csharp
// Controllers/VideoController.cs
[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly Kernel _kernel;

    public VideoController(Kernel kernel) => _kernel = kernel;

    /// <summary>Sube un video, extrae audio, lo transcribe con Whisper y devuelve el texto.</summary>
    [HttpPost("transcribir")]
    [RequestSizeLimit(100_000_000)] // 100MB
    public async Task<IActionResult> TranscribirVideo(IFormFile video)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "curso-sk", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        var videoPath = Path.Combine(tempDir, video.FileName);
        var audioPath = Path.Combine(tempDir, "audio.mp3");
        var audioComprimido = Path.Combine(tempDir, "audio_compressed.mp3");

        // Guardar video subido
        await using (var stream = new FileStream(videoPath, FileMode.Create))
            await video.CopyToAsync(stream);

        // Extraer audio
        var videoPlugin = _kernel.Plugins["VideoPlugin"];
        await _kernel.InvokeAsync(videoPlugin["extraer_audio"], new KernelArguments
        {
            ["videoPath"] = videoPath, ["outputPath"] = audioPath
        });

        // Comprimir si > 25MB
        var audioFinal = audioPath;
        if (new FileInfo(audioPath).Length > 25 * 1024 * 1024)
        {
            await _kernel.InvokeAsync(videoPlugin["comprimir_audio"], new KernelArguments
            {
                ["audioPath"] = audioPath, ["outputPath"] = audioComprimido
            });
            audioFinal = audioComprimido;
        }

        // Transcribir con Whisper
        #pragma warning disable SKEXP0010
        var whisper = _kernel.GetRequiredService<IAudioToTextService>();
        var audioBytes = await System.IO.File.ReadAllBytesAsync(audioFinal);
        var transcription = await whisper.GetTextContentAsync(
            new AudioContent(audioBytes, "audio/mpeg"));
        #pragma warning restore SKEXP0010

        // Limpiar archivos temporales
        Directory.Delete(tempDir, true);

        return Ok(new { transcripcion = transcription.Text });
    }

    /// <summary>Corta un clip de un video subido.</summary>
    [HttpPost("cortar")]
    [RequestSizeLimit(100_000_000)]
    public async Task<IActionResult> CortarClip(
        IFormFile video, 
        [FromQuery] string inicio, 
        [FromQuery] string duracion)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), "curso-sk", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        var videoPath = Path.Combine(tempDir, video.FileName);
        var clipPath = Path.Combine(tempDir, "clip.mp4");

        await using (var stream = new FileStream(videoPath, FileMode.Create))
            await video.CopyToAsync(stream);

        var videoPlugin = _kernel.Plugins["VideoPlugin"];
        await _kernel.InvokeAsync(videoPlugin["cortar_clip"], new KernelArguments
        {
            ["videoPath"] = videoPath, ["inicio"] = inicio,
            ["duracion"] = duracion, ["outputPath"] = clipPath
        });

        var clipBytes = await System.IO.File.ReadAllBytesAsync(clipPath);
        Directory.Delete(tempDir, true);
        return File(clipBytes, "video/mp4", "clip.mp4");
    }
}
```

> **Probar desde Swagger:**
> - `POST /api/video/transcribir` → subir un archivo de video → recibir transcripción
> - `POST /api/video/cortar?inicio=00:01:00&duracion=00:00:30` → subir video → recibir clip

**🔗 Conexión con el Proyecto Real:**
Se compara con el flujo de transcripción de audio de WhatsApp del laboratorio:
```csharp
// CotizacionIntegration.cs — Descarga audio OGG → Transcripción con gpt-4o-transcribe
byte[] audioBytes = await client.GetByteArrayAsync(audioUrl);
// POST multipart a Azure OpenAI /audio/transcriptions
```

---

#### 📅 Sesión 8 (Miércoles) — Técnicas de Prompting y Plantillas

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Prompting avanzado y Prompt Templates |
| **Secciones Udemy** | Sección 8 completa (7 lecciones) |
| **Subtemas** | Zero-Shot, Few-Shot, Chain of Thought, templates SK/Handlebars/Liquid, variables, archivos YAML |
| **Herramientas** | Semantic Kernel (Prompt Templates), HandlebarsPromptTemplateFactory, LiquidPromptTemplateFactory, ASP.NET Core, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (1h):**
- Técnicas de Prompting:
  - **Zero-Shot**: sin ejemplos, instrucción directa
  - **One-Shot / Few-Shot**: con 1 o más ejemplos
  - **Chain of Thought (CoT)**: razonamiento paso a paso
  - **Generated Knowledge**: generar conocimiento antes de responder
- Prompt Templates en SK:
  - **SK nativo**: `{{$variable}}`, `{{plugin.function}}`
  - **Handlebars**: `{{variable}}`, `{{#if}}`, `{{#each}}`
  - **Liquid**: `{{variable}}`, `{% if %}`, `{% for %}`
- Convertir prompts a `ChatHistory` con tags `<message role="system/user/assistant">`
- Archivos YAML: separar la definición del prompt del código
- Cargar prompts desde la carpeta `Prompts/` del proyecto API

**Actividad Práctica (2h):**

> **Ejercicio 8.1 — Endpoint con distintas técnicas de Prompting**

```csharp
// Controllers/PromptingController.cs
[ApiController]
[Route("api/[controller]")]
public class PromptingController : ControllerBase
{
    private readonly Kernel _kernel;
    public PromptingController(Kernel kernel) => _kernel = kernel;

    /// <summary>Clasificación de sentimiento con Zero-Shot.</summary>
    [HttpPost("zero-shot")]
    public async Task<IActionResult> ZeroShot([FromBody] TextoRequest request)
    {
        var result = await _kernel.InvokePromptAsync(
            $"Clasifica como positivo, negativo o neutro: '{request.Texto}'");
        return Ok(new { clasificacion = result.ToString() });
    }

    /// <summary>Clasificación con Few-Shot (ejemplos incluidos en el prompt).</summary>
    [HttpPost("few-shot")]
    public async Task<IActionResult> FewShot([FromBody] TextoRequest request)
    {
        var result = await _kernel.InvokePromptAsync($"""
            Clasifica el sentimiento:
            Texto: "Me encantó la comida" → Positivo
            Texto: "Fue terrible" → Negativo  
            Texto: "Llegó a tiempo" → Neutro
            Texto: "{request.Texto}" →
            """);
        return Ok(new { clasificacion = result.ToString() });
    }

    /// <summary>Resolución de problemas con Chain of Thought.</summary>
    [HttpPost("chain-of-thought")]
    public async Task<IActionResult> ChainOfThought([FromBody] TextoRequest request)
    {
        var result = await _kernel.InvokePromptAsync($"""
            Resuelve paso a paso, mostrando tu razonamiento:
            {request.Texto}
            
            Piensa paso a paso antes de dar la respuesta final.
            """);
        return Ok(new { respuesta = result.ToString() });
    }
}

public record TextoRequest(string Texto);
```

> **Ejercicio 8.2 — Endpoint con Prompt Templates (SK, Handlebars, Liquid)**

```csharp
/// <summary>Traduce texto usando el formato de template especificado.</summary>
[HttpPost("traducir")]
public async Task<IActionResult> Traducir([FromBody] TraducirRequest request)
{
    KernelFunction func;

    switch (request.TemplateFormat?.ToLower())
    {
        case "handlebars":
            var hb = """
                <message role="system">Eres un traductor profesional.</message>
                <message role="user">Traduce de {{idiomaOrigen}} a {{idiomaDestino}}: {{texto}}</message>
                """;
            func = _kernel.CreateFunctionFromPrompt(
                new PromptTemplateConfig(hb) { TemplateFormat = "handlebars" },
                new HandlebarsPromptTemplateFactory());
            break;
        case "liquid":
            var lq = """
                <message role="system">Eres un traductor profesional.</message>
                <message role="user">Traduce de {{idiomaOrigen}} a {{idiomaDestino}}: {{texto}}</message>
                """;
            func = _kernel.CreateFunctionFromPrompt(
                new PromptTemplateConfig(lq) { TemplateFormat = "liquid" },
                new LiquidPromptTemplateFactory());
            break;
        default: // SK nativo
            func = _kernel.CreateFunctionFromPrompt(
                "Traduce de {{$idiomaOrigen}} a {{$idiomaDestino}}: {{$texto}}");
            break;
    }

    var result = await _kernel.InvokeAsync(func, new KernelArguments
    {
        ["idiomaOrigen"] = request.IdiomaOrigen,
        ["idiomaDestino"] = request.IdiomaDestino,
        ["texto"] = request.Texto
    });
    return Ok(new { traduccion = result.ToString(), formato = request.TemplateFormat ?? "sk" });
}

public record TraducirRequest(string Texto, string IdiomaOrigen, string IdiomaDestino, string? TemplateFormat);
```

> **Ejercicio 8.3 — Prompts en Archivos YAML cargados desde el proyecto**

```yaml
# Prompts/ClasificarIntencion.yaml (en la raíz del proyecto API)
name: ClasificarIntencion
description: Clasifica la intención del usuario
template_format: handlebars
template: |
  <message role="system">
  Clasifica la siguiente consulta en una categoría:
  - consulta_precio: preguntas sobre precios o costos
  - agendar_cita: solicitudes de citas o agendamiento  
  - informacion: preguntas generales
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

```csharp
/// <summary>Clasifica la intención del usuario usando prompt YAML.</summary>
[HttpPost("clasificar-intencion")]
public async Task<IActionResult> ClasificarIntencion([FromBody] TextoRequest request)
{
    var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "ClasificarIntencion.yaml");
    var yamlContent = await System.IO.File.ReadAllTextAsync(yamlPath);
    var promptConfig = KernelFunctionYaml.FromPromptYaml(yamlContent);
    var function = _kernel.CreateFunctionFromPrompt(promptConfig);
    var result = await _kernel.InvokeAsync(function, 
        new KernelArguments { ["consulta"] = request.Texto });
    return Ok(new { intencion = result.ToString() });
}
```

> **Probar desde Swagger:**
> - `POST /api/prompting/zero-shot` → `{ "texto": "El servicio fue excelente" }`
> - `POST /api/prompting/few-shot` → `{ "texto": "No me gustó nada" }`
> - `POST /api/prompting/traducir` → `{ "texto": "Hola mundo", "idiomaOrigen": "español", "idiomaDestino": "inglés", "templateFormat": "handlebars" }`
> - `POST /api/prompting/clasificar-intencion` → `{ "texto": "¿Cuánto cuesta una consulta?" }`

**🔗 Conexión con el Proyecto Real:**
Se muestran las plantillas de prompts del laboratorio clínico:
```
// Prompts/LabAnalisis/DeterminarIntencion/skprompt.txt
Eres un especialista en clasificar consultas de usuarios para un laboratorio clínico.
Clasifica en: informaciongeneral | buscaranalisis | cotizaranalisis | extracciondatos | promociones
Consulta: {{$consulta}}

// Prompts/LabAnalisis/DeterminarIntencion/config.json
{ "completion": { "max_tokens": 50, "temperature": 0.0 } }

// Prompts/LabAnalisis/BuscarAnalisis/skprompt.txt (Few-Shot)
<message role="user">Necesito saber sobre las pruebas de glucosa y colesterol.</message>
<message role="assistant">Glucosa en Sangre|Perfil Lipídico</message>
<message role="user">{{$consulta}}</message>
```

---

### SEMANA 4

---

#### 📅 Sesión 9 (Lunes) — Workshop: Generador de Podcasts

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Proyecto Completo #4: Generador de Podcasts con SK |
| **Secciones Udemy** | Sección 9 completa (8 lecciones) |
| **Subtemas** | Recolección de datos, conversión HTML→Markdown, lluvia de ideas con IA, generación de scripts, TTS para podcast |
| **Herramientas** | Semantic Kernel, ReverseMarkdown, TTS Azure OpenAI, plantillas YAML, ASP.NET Core, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (30min):**
- Arquitectura del pipeline: Input → Markdown → Brainstorm → Script → Conclusión → Audio
- ReverseMarkdown: librería para convertir HTML a Markdown limpio
- Cadena de prompts: cada paso alimenta al siguiente
- TTS para podcast: selección de voces, concatenación de audio
- Servicio Layer: encapsular el pipeline complejo en un servicio inyectable

**Actividad Práctica — Proyecto Completo (2h 30m):**

> **Proyecto 4: Podcast Generator API**
> Agregar `PodcastService` y `PodcastController` a la Web API.

```csharp
// Services/PodcastService.cs
public class PodcastService
{
    private readonly Kernel _kernel;
    private readonly HttpClient _httpClient;

    public PodcastService(Kernel kernel, IHttpClientFactory httpClientFactory)
    {
        _kernel = kernel;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> DescargarYConvertirAMarkdown(string[] urls)
    {
        var converter = new ReverseMarkdown.Converter();
        var sb = new StringBuilder();
        foreach (var url in urls)
        {
            var html = await _httpClient.GetStringAsync(url.Trim());
            sb.AppendLine(converter.Convert(html));
        }
        return sb.ToString();
    }

    public async Task<string> GenerarLluviaDeIdeas(string tema, string contenido, string formato)
    {
        var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "PodcastBrainstorm.yaml");
        var func = _kernel.CreateFunctionFromPrompt(
            KernelFunctionYaml.FromPromptYaml(await File.ReadAllTextAsync(yamlPath)));
        var result = await _kernel.InvokeAsync(func, new KernelArguments
        {
            ["tema"] = tema, ["contenido"] = contenido, ["formato"] = formato
        });
        return result.ToString();
    }

    public async Task<string> GenerarScript(string ideas, string formato)
    {
        var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "PodcastScript.yaml");
        var func = _kernel.CreateFunctionFromPrompt(
            KernelFunctionYaml.FromPromptYaml(await File.ReadAllTextAsync(yamlPath)));
        var result = await _kernel.InvokeAsync(func, new KernelArguments
        {
            ["ideas"] = ideas, ["formato"] = formato
        });
        return result.ToString();
    }

    public async Task<byte[]> GenerarAudio(string script)
    {
        #pragma warning disable SKEXP0010
        var audioService = _kernel.GetRequiredService<ITextToAudioService>();
        var audioContent = await audioService.GetAudioContentAsync(
            script,
            new OpenAITextToAudioExecutionSettings("tts-1-hd") { Voice = "nova", Speed = 1.0f });
        return audioContent.Data!.ToArray();
        #pragma warning restore SKEXP0010
    }
}

// Registrar en Program.cs:
// builder.Services.AddHttpClient();
// builder.Services.AddSingleton<PodcastService>();
```

```csharp
// Controllers/PodcastController.cs
[ApiController]
[Route("api/[controller]")]
public class PodcastController : ControllerBase
{
    private readonly PodcastService _podcastService;

    public PodcastController(PodcastService podcastService) => _podcastService = podcastService;

    /// <summary>Genera el script del podcast a partir de URLs y un tema.</summary>
    [HttpPost("script")]
    public async Task<IActionResult> GenerarScript([FromBody] PodcastRequest request)
    {
        var markdown = await _podcastService.DescargarYConvertirAMarkdown(request.Urls);
        var ideas = await _podcastService.GenerarLluviaDeIdeas(
            request.Tema, markdown, request.Formato);
        var script = await _podcastService.GenerarScript(ideas, request.Formato);
        return Ok(new { ideas, script });
    }

    /// <summary>Pipeline completo: genera script + audio MP3 del podcast.</summary>
    [HttpPost("generar")]
    public async Task<IActionResult> GenerarPodcastCompleto([FromBody] PodcastRequest request)
    {
        var markdown = await _podcastService.DescargarYConvertirAMarkdown(request.Urls);
        var ideas = await _podcastService.GenerarLluviaDeIdeas(
            request.Tema, markdown, request.Formato);
        var script = await _podcastService.GenerarScript(ideas, request.Formato);
        var audioBytes = await _podcastService.GenerarAudio(script);
        return File(audioBytes, "audio/mpeg", $"podcast_{DateTime.Now:yyyyMMdd}.mp3");
    }
}

public record PodcastRequest(string[] Urls, string Tema, string Formato = "Conversacional");
```

**Archivos YAML del proyecto:**

```yaml
# Prompts/PodcastBrainstorm.yaml
name: PodcastBrainstorm
template_format: handlebars
template: |
  <message role="system">
  Eres un productor de podcasts experto. Genera una lluvia de ideas
  para un episodio de podcast en formato {{formato}} sobre: {{tema}}
  
  Contenido de referencia:
  {{contenido}}
  
  Genera 5-7 puntos clave con subtemas interesantes.
  </message>
execution_settings:
  default:
    max_tokens: 1000
    temperature: 0.8
input_variables:
  - name: tema
    is_required: true
  - name: contenido
    is_required: true
  - name: formato
    is_required: true
```

> **Probar desde Swagger:**
> - `POST /api/podcast/script` → `{ "urls": ["https://learn.microsoft.com/..."], "tema": "Semantic Kernel", "formato": "Conversacional" }`
> - `POST /api/podcast/generar` → mismo body → descarga archivo MP3

---

#### 📅 Sesión 10 (Miércoles) — Vector Stores, Embeddings y Búsqueda Semántica

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Memoria: Embeddings, Vector Stores y Búsqueda por Texto (RAG) |
| **Secciones Udemy** | Sección 10 completa (8 lecciones) |
| **Subtemas** | Embeddings, modelo de datos vectorial, InMemoryVectorStore, búsqueda vectorial, text search, plugins RAG con Function Calling |
| **Herramientas** | Semantic Kernel (Vector Stores), text-embedding-ada-002, InMemoryVectorStore, VectorStoreTextSearch, ASP.NET Core, Swagger |
| **Duración** | 3 horas |

**Contenido Teórico (1h):**
- **Embeddings**: representaciones vectoriales de texto en espacio n-dimensional
- **Vector Stores**: almacenes especializados para búsqueda por similitud vectorial
- Modelo de datos: `[VectorStoreRecordKey]`, `[VectorStoreRecordData]`, `[VectorStoreRecordVector]`
- `ITextEmbeddingGenerationService`: genera embeddings con text-embedding-ada-002
- `InMemoryVectorStore`: almacén vectorial en memoria (desarrollo/pruebas)
- **Búsqueda vectorial**: encontrar documentos similares por cercanía en el espacio vectorial
- **Text Search**: `VectorStoreTextSearch` — búsqueda semántica como plugin
- **RAG (Retrieval-Augmented Generation)**: combinar búsqueda + LLM para respuestas fundamentadas

**Actividad Práctica (2h):**

> **Ejercicio 10.1 — Modelo de datos, embeddings y endpoints CRUD**

```csharp
// Models/DocumentoFAQ.cs
public class DocumentoFAQ
{
    [VectorStoreRecordKey]
    public string Id { get; set; }
    
    [VectorStoreRecordData]
    public string Pregunta { get; set; }
    
    [VectorStoreRecordData]
    public string Respuesta { get; set; }
    
    [VectorStoreRecordData]
    public string Categoria { get; set; }
    
    [VectorStoreRecordVector(1536)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
```

```csharp
// Services/VectorStoreService.cs
public class VectorStoreService
{
    private readonly Kernel _kernel;
    private readonly IVectorStoreRecordCollection<string, DocumentoFAQ> _collection;

    public VectorStoreService(Kernel kernel)
    {
        _kernel = kernel;
        var vectorStore = new InMemoryVectorStore();
        _collection = vectorStore.GetCollection<string, DocumentoFAQ>("faq");
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

// Registrar en Program.cs:
// kernelBuilder.AddOpenAITextEmbeddingGeneration("text-embedding-ada-002", openAiKey);
// builder.Services.AddSingleton<VectorStoreService>();
```

> **Ejercicio 10.2 — Endpoints de búsqueda vectorial**

```csharp
// Controllers/RAGController.cs
[ApiController]
[Route("api/[controller]")]
public class RAGController : ControllerBase
{
    private readonly VectorStoreService _vectorService;
    private readonly Kernel _kernel;

    public RAGController(VectorStoreService vectorService, Kernel kernel)
    {
        _vectorService = vectorService;
        _kernel = kernel;
    }

    /// <summary>Agrega una FAQ al Vector Store (genera embedding automáticamente).</summary>
    [HttpPost("faq")]
    public async Task<IActionResult> AgregarFAQ([FromBody] FAQRequest request)
    {
        await _vectorService.AgregarFAQ(request.Pregunta, request.Respuesta, request.Categoria);
        return Ok(new { mensaje = "FAQ agregada exitosamente" });
    }

    /// <summary>Carga FAQs de ejemplo para demostración.</summary>
    [HttpPost("faq/seed")]
    public async Task<IActionResult> SeedFAQs()
    {
        var faqs = new[]
        {
            ("¿Cuál es el horario de atención?", "Lunes a Viernes de 8:00 AM a 5:00 PM", "Horarios"),
            ("¿Aceptan tarjetas de crédito?", "Sí, aceptamos Visa, Mastercard y American Express", "Pagos"),
            ("¿Cuánto tiempo tarda el resultado?", "Resultados disponibles en 24-48 horas", "Resultados"),
            ("¿Necesito cita previa?", "No, atendemos por orden de llegada", "Citas"),
            ("¿Dónde están ubicados?", "Boulevard Morazán, Torre 1, Piso 3, Tegucigalpa", "Ubicación")
        };
        foreach (var (p, r, c) in faqs) await _vectorService.AgregarFAQ(p, r, c);
        return Ok(new { mensaje = $"{faqs.Length} FAQs cargadas" });
    }

    /// <summary>Busca FAQs similares a la consulta (búsqueda vectorial pura).</summary>
    [HttpPost("buscar")]
    public async Task<IActionResult> BuscarSimilares([FromBody] TextoRequest request)
    {
        var resultados = await _vectorService.BuscarSimilares(request.Texto);
        return Ok(resultados.Select(r => new {
            pregunta = r.Doc.Pregunta,
            respuesta = r.Doc.Respuesta,
            categoria = r.Doc.Categoria,
            score = r.Score
        }));
    }
}

public record FAQRequest(string Pregunta, string Respuesta, string Categoria);
```

> **Ejercicio 10.3 — RAG: Text Search Plugin + Function Calling vía endpoint**

```csharp
/// <summary>
/// RAG: el LLM busca automáticamente en la base de conocimiento y responde.
/// Combina Vector Store + Function Calling para respuestas fundamentadas.
/// </summary>
[HttpPost("consultar")]
public async Task<IActionResult> ConsultarRAG([FromBody] TextoRequest request)
{
    #pragma warning disable SKEXP0010
    var embeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
    var textSearch = new VectorStoreTextSearch<DocumentoFAQ>(
        _vectorService.Collection, embeddingService);
    var searchPlugin = textSearch.CreateWithGetTextSearchResults("BuscarFAQ");
    _kernel.Plugins.Add(searchPlugin);
    #pragma warning restore SKEXP0010

    var settings = new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    var result = await _kernel.InvokePromptAsync(
        $"Un cliente pregunta: '{request.Texto}'. Busca la respuesta en nuestra base de conocimiento y responde amablemente.",
        new KernelArguments(settings));

    // Limpiar plugin temporal
    _kernel.Plugins.Remove(_kernel.Plugins["BuscarFAQ"]);

    return Ok(new { respuesta = result.ToString() });
}
```

> **Probar desde Swagger — Flujo completo de RAG:**
> 1. `POST /api/rag/faq/seed` → cargar FAQs de ejemplo
> 2. `POST /api/rag/buscar` → `{ "texto": "¿A qué hora abren?" }` → búsqueda vectorial directa
> 3. `POST /api/rag/consultar` → `{ "texto": "¿Puedo pagar con tarjeta?" }` → RAG con Function Calling
> 4. `POST /api/rag/faq` → agregar nuevas FAQs en tiempo real sin redeployar

**🔗 Conexión con el Proyecto Real:**
En producción, el laboratorio usa clasificación de intenciones (similar a búsqueda semántica) para enrutar las consultas:
```csharp
// Prompts/LabAnalisis/DeterminarIntencion — Clasificación por intención
// En producción se podría reemplazar con Vector Store para FAQ dinámicas
// que se actualicen sin redeployar el sistema
```

---

## Resumen de Módulos de la API por Sesión

| Sesión | Módulo / Controller | Endpoints Principales | Tecnologías Clave |
|:------:|----------|------|-------------------|
| 1 | `KernelController` | `POST /api/kernel/prompt`, `/prompt/configurado` | SK + ASP.NET Core + Swagger |
| 2 | `MultimodalController` | `POST /api/multimodal/stream`, `/imagen`, `/tts`, `/stt` | DALL-E + TTS + Whisper + SSE |
| 3 | `BlogController` | `POST /api/blog/generar`, `/publicar` | SK + WordPress REST API |
| 4 | `ChatController` | `POST /api/chat/{id}/mensaje`, `/imagen`, `/imagen/upload` | ChatHistory + ImageContent |
| 5 | `AgentController` | `POST /api/agent/consultar`, `GET /api/agent/plugins` | Plugins + Function Calling |
| 6 | `AgentController` | `POST /api/agent/plugins/openapi` | Filters + OpenAPI + DI |
| 7 | `VideoController` | `POST /api/video/transcribir`, `/cortar` | FFmpeg + Whisper + CliWrap |
| 8 | `PromptingController` | `POST /api/prompting/zero-shot`, `/few-shot`, `/traducir`, `/clasificar-intencion` | Prompt Templates + YAML |
| 9 | `PodcastController` | `POST /api/podcast/script`, `/generar` | ReverseMarkdown + TTS + YAML |
| 10 | `RAGController` | `POST /api/rag/faq`, `/buscar`, `/consultar` | Vector Stores + Embeddings + RAG |

---

## Paquetes NuGet Requeridos

```xml
<!-- ASP.NET Core + Swagger -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.9.0" />

<!-- Semantic Kernel base -->
<PackageReference Include="Microsoft.SemanticKernel" Version="1.47.0" />

<!-- Plugins y conectores -->
<PackageReference Include="Microsoft.SemanticKernel.Plugins.Core" Version="1.47.0-alpha" />
<PackageReference Include="Microsoft.SemanticKernel.Plugins.OpenApi" Version="1.47.0-alpha" />
<PackageReference Include="Microsoft.SemanticKernel.Connectors.InMemory" Version="1.47.0-alpha" />

<!-- Prompt Templates -->
<PackageReference Include="Microsoft.SemanticKernel.PromptTemplates.Handlebars" Version="1.47.0" />
<PackageReference Include="Microsoft.SemanticKernel.PromptTemplates.Liquid" Version="1.47.0" />

<!-- Utilidades -->
<PackageReference Include="CliWrap" Version="3.6.6" />
<PackageReference Include="ReverseMarkdown" Version="4.6.0" />
```

---

## Mapa de Competencias vs Sesiones

| Competencia | Sesiones | Evaluación |
|-------------|:--------:|------------|
| Configurar Kernels (OpenAI / Azure OpenAI) | 1, 2 | Ejercicios 1.1, 1.2 |
| Chat Completion, Streaming, Imágenes, Audio | 2, 3 | Proyecto Blog Post |
| ChatHistory y estado conversacional | 4 | Proyecto Chat Multi-modal |
| Plugins nativos con Function Calling | 5, 6 | Ejercicios 5.1, 5.2, 6.1 |
| Procesamiento multimedia (FFmpeg + Whisper) | 7 | Proyecto Video Extractor |
| Técnicas de Prompting y Plantillas YAML | 8 | Ejercicios 8.1, 8.2, 8.3 |
| Generación de contenido con pipeline IA | 9 | Proyecto Podcast Generator |
| Vector Stores, Embeddings y RAG | 10 | Ejercicios 10.1, 10.2, 10.3 |

---

## Calendario Sugerido (5 Semanas)

| Semana | Lunes | Miércoles | Viernes |
|:------:|-------|-----------|---------|
| **1** | Sesión 1: Fundamentos + Kernel | Sesión 2: Servicios Multimodales | Sesión 3: Workshop Blog Post |
| **2** | Sesión 4: ChatHistory + Chat Multimodal | Sesión 5: Plugins + Function Calling | Sesión 6: Plugins Avanzados |
| **3** | Sesión 7: Workshop Video Extractor | Sesión 8: Prompting + Templates | — |
| **4** | Sesión 9: Workshop Podcast Generator | Sesión 10: Vector Stores + RAG | — |
| **5** | *Sesión de repaso / Proyecto final integrador (opcional)* | | |

---

## Estructura del Proyecto Web API (Incremental)

Un solo proyecto que crece sesión a sesión — cada clase agrega archivos al mismo proyecto:

```
📁 CursoSK.Api/
├── Program.cs                          (Sesión 1 — crece en cada sesión)
├── appsettings.json                    (Sesión 1 — config de LLM)
├── 📁 Controllers/
│   ├── KernelController.cs             (Sesión 1 — prompt básico)
│   ├── MultimodalController.cs         (Sesión 2 — streaming, imágenes, audio)
│   ├── BlogController.cs               (Sesión 3 — generación blog + WordPress)
│   ├── ChatController.cs               (Sesión 4 — chat con historia + multimodal)
│   ├── AgentController.cs              (Sesión 5-6 — plugins + Function Calling)
│   ├── VideoController.cs              (Sesión 7 — transcripción + corte de video)
│   ├── PromptingController.cs          (Sesión 8 — técnicas de prompting)
│   ├── PodcastController.cs            (Sesión 9 — generación de podcasts)
│   └── RAGController.cs                (Sesión 10 — Vector Stores + RAG)
├── 📁 Services/
│   ├── BlogService.cs                  (Sesión 3)
│   ├── ChatSessionService.cs           (Sesión 4)
│   ├── PodcastService.cs               (Sesión 9)
│   └── VectorStoreService.cs           (Sesión 10)
├── 📁 Plugins/
│   ├── ClimaPlugin.cs                  (Sesión 5)
│   ├── MathPlugin.cs                   (Sesión 5)
│   ├── WikipediaPlugin.cs              (Sesión 6)
│   └── VideoPlugin.cs                  (Sesión 7)
├── 📁 Filters/
│   └── LoggingFilter.cs                (Sesión 6)
├── 📁 Models/
│   └── DocumentoFAQ.cs                 (Sesión 10)
├── 📁 DTOs/
│   └── Requests.cs                     (Todos los records DTO)
├── 📁 Prompts/
│   ├── ClasificarIntencion.yaml        (Sesión 8)
│   ├── PodcastBrainstorm.yaml           (Sesión 9)
│   ├── PodcastScript.yaml              (Sesión 9)
│   └── PodcastConclusion.yaml           (Sesión 9)
└── 📁 ProyectoReferencia/
    ├── README.md                       (Descripción del sistema en producción)
    └── snippets/                       (Fragmentos clave del proyecto real)
        ├── KernelConfig.cs
        ├── CotizacionPlugin_sample.cs
        ├── FechaHoraPlugin.cs
        └── PromptTemplates/
```

> **Nota:** Al final del curso, los alumnos tienen **una API REST completa con ~20 endpoints documentados en Swagger** que cubre: prompts, streaming, imágenes, audio, chat con historia, plugins con Function Calling, procesamiento de video, técnicas de prompting, generación de podcasts y RAG con Vector Stores.
