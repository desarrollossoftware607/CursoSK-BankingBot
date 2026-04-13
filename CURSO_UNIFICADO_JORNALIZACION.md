# Curso Unificado: Semantic Kernel + Agentes de IA con Azure OpenAI

> **Este documento es la jornalización oficial del curso.**

---

## Información General

| Campo | Detalle |
|-------|---------|
| **Nombre del Curso** | Semantic Kernel + Agentes de IA con Azure OpenAI |
| **Duración** | 30 horas cronológicas (10 sesiones × 3 horas) |
| **Horario** | Lunes, Miércoles y Viernes — 3 horas por sesión |
| **Modalidad** | Formación práctica de alto nivel — "Aprender Haciendo" |
| **Requisitos** | Conocimientos básicos de C# / .NET, Visual Studio 2022 o VS Code con extensión C#, suscripción Azure (crédito $200 nuevos usuarios) |
| **Proyecto** | **CursoSK.Api** — API Web que crece sesión a sesión (puerto 5192) |
| **Repositorio** | Monorepo con ramas Git por sesión (`sesion/01` a `sesion/10`) |
| **Idioma** | Español |

---

## Objetivo General

Al finalizar el curso, los participantes serán capaces de **construir una API REST inteligente con ASP.NET Core y Semantic Kernel** que integre modelos de IA (Azure OpenAI) para: generar texto, imágenes y audio; crear plugins nativos con Function Calling; implementar técnicas avanzadas de prompting; utilizar Vector Stores para búsqueda semántica (RAG); y desplegar agentes de IA en producción con Azure App Service y Microsoft Foundry — todo desarrollado sobre **un único proyecto Web API que crece sesión a sesión**.

---

## Plataforma: Microsoft Foundry (antes Azure AI Studio)

> **Referencia oficial:** https://learn.microsoft.com/es-mx/azure/foundry/

Microsoft Foundry es la plataforma unificada de Azure para operaciones de IA empresarial:

| Componente | Propósito |
|---|---|
| **Foundry Models** | Modelos vendidos por Azure (GPT-4, GPT-4o, GPT-4.1-mini, DeepSeek-R1) |
| **Foundry Agent Service** | Orquestación y hospedaje de agentes de IA |
| **Foundry Tools** | Speech, Translator, Document Intelligence, Content Safety, Vision |
| **Foundry IQ** | Base de conocimiento conectada a agentes con citaciones |
| **Foundry Local** | Ejecución de LLMs en dispositivo local |

| Concepto Anterior | Concepto Actual en Foundry |
|---|---|
| Azure AI Studio / Azure AI Foundry | Microsoft Foundry |
| Assistants API (Agents v0.5/v1) | Responses API (Agents v2) |
| Hub + Azure OpenAI + Azure AI Services | Foundry resource (único, con proyectos) |

**Portal:** https://ai.azure.com

---

## Semantic Kernel — SDK de Orquestación

> **Referencia oficial:** https://learn.microsoft.com/en-us/semantic-kernel/

| Concepto | Descripción |
|---|---|
| **Kernel** | Contenedor central de servicios y plugins (patrón DI) |
| **Plugins** | Funciones nativas en C# expuestas al LLM con `[KernelFunction]` |
| **Memory / Vector Store** | Conectores para bases de datos vectoriales |
| **Agent Framework** | `ChatCompletionAgent`, `OpenAIAssistantAgent`, orquestación multi-agente |
| **Process Framework** | Flujos de trabajo con Steps, Events, State |

**Paquetes NuGet principales:**

| Paquete | Versión | Propósito |
|---|---|---|
| `Microsoft.SemanticKernel` | 1.48.0 | Core del SDK |
| `Microsoft.SemanticKernel.Yaml` | 1.48.0 | Templates YAML |
| `Microsoft.SemanticKernel.Plugins.Core` | 1.48.0-alpha | Plugins pre-construidos |
| `Microsoft.SemanticKernel.PromptTemplates.Handlebars` | 1.48.0 | Templates Handlebars |
| `Microsoft.SemanticKernel.Agents.Core` | 1.48.0 | ChatCompletionAgent |

---

## Competencias a Desarrollar

| # | Competencia | Sesión |
|---|-------------|--------|
| 1 | Configurar Kernels de Semantic Kernel con Azure OpenAI | 1 |
| 2 | Crear recursos Azure OpenAI desde portal y CLI | 1, 2, 7 |
| 3 | Utilizar Chat Completion, Streaming, generación de imágenes y audio | 2 |
| 4 | Implementar Blog Generator y Chat History con sesiones | 3 |
| 5 | Desarrollar Chat Multimodal con imágenes y persistencia EF Core | 4 |
| 6 | Crear plugins nativos con `[KernelFunction]` y Function Calling automático | 5 |
| 7 | Implementar filtros, plugins avanzados y OpenAPI | 6 |
| 8 | Diseñar prompts con técnicas avanzadas y plantillas YAML/Handlebars | 7 |
| 9 | Construir pipeline de generación de contenido (Podcast) e intro a embeddings | 8 |
| 10 | Implementar RAG completo con Vector Store y Agent Framework | 9 |
| 11 | Desplegar API en Azure App Service y configurar Microsoft Foundry | 10 |

---

## Proyecto del Curso — CursoSK.Api (Puerto 5192)

Un único proyecto Web API que crece sesión a sesión, acumulando funcionalidad:

| Módulo | Controller | Endpoints | Sesión |
|---|---|---|---|
| Kernel Core | `KernelController` | `/api/kernel/prompt`, `/prompt/configurado` | 1 |
| Multimodal | `MultimodalController` | `/api/multimodal/stream` (SSE) | 2 |
| Blog Generator | `BlogController` | `/api/blog/generar` | 3 |
| Chat | `ChatController` | `/api/chat/{id}/mensaje`, `/imagen`, `/historial` | 3-4 |
| Function Calling | `AgentController` | `/api/agent/consultar`, `/plugins` | 5-6 |
| Prompting | `PromptingController` | `/api/prompting/zero-shot`, `/few-shot`, `/chain-of-thought`, `/yaml` | 7 |
| RAG | `RAGController` | `/api/rag/indexar`, `/buscar`, `/consultar`, `/seed` | 8-9 |

---

## Proyecto de Referencia en Producción

El curso se complementa con un **sistema real en producción**: un agente conversacional multicanal (WhatsApp, Messenger, Chat Web) para un laboratorio clínico:

| Concepto del Curso | Implementación en Producción |
|---|---|
| Creación del Kernel con Azure OpenAI | `Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(...)` + Multi-LLM |
| Plugins nativos con `[KernelFunction]` | 6 plugins: Cotización, Citas, Domicilio, Análisis, Laboratorio, FechaHora |
| Function Calling automático | `ToolCallBehavior.AutoInvokeKernelFunctions` con 20+ funciones |
| Chat History y estado conversacional | `ConversationSession` por número de teléfono + `ChatHistory` |
| Transcripción de audio (Whisper) | Azure OpenAI `gpt-4o-mini-transcribe` vía REST multipart |
| Prompt Templates con variables | `skprompt.txt` + `config.json` para clasificación de intenciones |

---

## Ramas Git por Sesión

Cada rama parte de la rama anterior — el código es **acumulativo**.

| Rama | Contenido Acumulado |
|---|---|
| `sesion/01` | Setup del proyecto + `KernelController` + scripts Azure básicos |
| `sesion/02` | + `MultimodalController` + deployment Whisper |
| `sesion/03` | + `BlogController` + `ChatController` (texto) + services |
| `sesion/04` | + Chat multimodal con imágenes + EF Core + modelos + seed data |
| `sesion/05` | + `ClimaPlugin` + `MathPlugin` + `AgentController` |
| `sesion/06` | + `LoggingFilter` + plugins avanzados + OpenAPI |
| `sesion/07` | + `PromptingController` + templates YAML/Handlebars + deployment embeddings |
| `sesion/08` | + Intro embeddings + `VectorStoreService` + `DocumentoVectorial` |
| `sesion/09` | + `RAGController` + indexación + búsqueda vectorial + Agent Framework |
| `sesion/10` | + Deploy App Service + Foundry config + producción |
| `main` | Estado final completo (merge de sesion/10) |

```bash
# Cambiar a la rama de una sesión específica
git checkout sesion/05
dotnet build CursoSK.Api/CursoSK.Api.csproj
```

---

## Scripts Azure (PowerShell)

Ubicados en `Scripts/Azure/`. Cada script incluye instrucciones CLI **y** pasos equivalentes en el Portal de Azure como comentarios.

| Script | Sesión | Propósito |
|---|---|---|
| `00-variables.ps1` | — | Variables compartidas y funciones auxiliares |
| `01-crear-recurso-openai.ps1` | 1 | Resource Group + Azure OpenAI + deployment de chat |
| `02-crear-deployment-whisper.ps1` | 2 | Deployment de audio (Whisper/TTS) |
| `07-crear-deployment-embedding.ps1` | 7 | Deployment de embeddings (text-embedding-3-small) |
| `09-crear-ai-search.ps1` | 9 | Azure AI Search (opcional, tier free) |
| `10-deploy-app-service.ps1` | 10 | Deploy a Azure App Service |
| `10-foundry-setup.ps1` | 10 | Configuración de Microsoft Foundry (manual) |

---

---

## JORNALIZACIÓN DETALLADA — 10 SESIONES

---

---

### 📅 SESIÓN 1 — Fundamentos de Semantic Kernel + Setup Azure (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Fundamentos de Semantic Kernel + Configuración del Entorno Azure |
| **Subtemas** | Qué es SK, componentes, arquitectura Kernel→Services→Plugins, Microsoft Foundry overview, OpenAI vs Azure OpenAI, patrón Builder, configuración de Kernels |
| **Herramientas** | Visual Studio 2022 / VS Code, NuGet, Azure Portal, Azure CLI, Swagger |
| **Rama Git** | `sesion/01` |
| **Script Azure** | `01-crear-recurso-openai.ps1` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (1h)

**¿Qué es Semantic Kernel?**
- SDK open-source de Microsoft para integrar LLMs en aplicaciones C#, Python o Java
- Actúa como orquestador central: coordina modelos de IA, plugins y servicios externos
- Arquitectura: `Kernel` → `Services` → `Plugins` → `Memory` → `Agents`

**Componentes principales:**
- **Kernel**: Contenedor central con patrón DI (Dependency Injection)
- **AI Services**: Chat Completion, Image Generation, Text-to-Audio, Audio-to-Text, Embeddings
- **Plugins**: Funciones nativas en C# expuestas al LLM
- **Prompt Templates**: Plantillas con variables (SK nativo, Handlebars, Liquid, YAML)
- **Filters**: Interceptores para logging, auditoría, seguridad

**OpenAI vs Azure OpenAI:**

| Aspecto | OpenAI | Azure OpenAI |
|---|---|---|
| Autenticación | API Key | API Key + Endpoint |
| Configuración SK | `AddOpenAIChatCompletion(modelId, apiKey)` | `AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey)` |
| Naming | Se usa `modelId` (nombre del modelo) | Se usa `deploymentName` (nombre del deployment, NO del modelo) |
| Compliance | Estándar | Enterprise (GDPR, SOC2, etc.) |

**Microsoft Foundry — Visión General:**
- Plataforma unificada de Azure para IA empresarial (evolución de Azure AI Studio)
- Portal: https://ai.azure.com
- Se pueden crear proyectos, desplegar modelos, usar Playground, crear índices vectoriales
- Foundry Agent Service: agentes hospedados, de solicitud y de flujo de trabajo

---

#### Bloque Azure Setup (30 min)

**🔧 Crear recursos Azure OpenAI — 3 opciones:**

**Opción A — Portal de Azure (portal.azure.com):**
1. Ir a https://portal.azure.com → **Create a resource** → buscar "Azure OpenAI"
2. Configurar:
   - **Subscription**: Seleccionar su suscripción
   - **Resource Group**: Crear nuevo (ej: `rg-cursosk`)
   - **Region**: `East US` (verificar disponibilidad de modelos)
   - **Name**: Nombre único globalmente (ej: `cursosk-openai`)
   - **Pricing tier**: `Standard S0`
3. **Networking**: "All networks" (para desarrollo)
4. **Review + Create** → **Create** → esperar despliegue
5. Ir al recurso → **Go to Microsoft Foundry portal** (o directamente a https://ai.azure.com)
6. **Deployments** → **+ Deploy model** → **Deploy base model**
7. Seleccionar `gpt-35-turbo-16k` (o `gpt-4o-mini` si disponible)
   - Deployment name: `gpt-35-turbo-16k` (este nombre se usa en el código)
   - Deployment type: `Standard`
   - TPM: ajustar según necesidad
8. **Deploy** → esperar estado `Succeeded`
9. Copiar **Endpoint** y **API Key** desde el recurso

**Opción B — Portal de Foundry (ai.azure.com):**
1. Ir a https://ai.azure.com con su cuenta de Azure
2. Activar toggle **"New Foundry"** si aparece
3. **+ New project** → asignar nombre → seleccionar/crear Foundry resource
4. **Build > Models + endpoints** → **Deploy model > Deploy base model**
5. Seleccionar modelo, configurar deployment name y TPM → **Deploy**

**Opción C — Azure CLI (PowerShell):**
```powershell
cd Scripts/Azure
. .\00-variables.ps1
.\01-crear-recurso-openai.ps1
```

> ⚠️ **IMPORTANTE:** El `deploymentName` es lo que se usa en las llamadas API, NO el nombre del modelo.

---

#### Bloque Práctico (1h 30min)

**Ejercicio 1.1 — Crear el proyecto Web API + Semantic Kernel**

```bash
dotnet new webapi -n CursoSK.Api -controllers
cd CursoSK.Api
dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.4
```

**Ejercicio 1.2 — Configurar Kernel en Program.cs**

```csharp
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

```json
// appsettings.json
{
  "LLMSettings": {
    "Provider": "azure",
    "AzureOpenAI": {
      "DeploymentName": "gpt-35-turbo-16k",
      "Endpoint": "https://tu-recurso.openai.azure.com/",
      "ApiKey": "TU-API-KEY-AQUI"
    }
  }
}
```

**Ejercicio 1.3 — Primer Controller: KernelController**

```csharp
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
        var settings = new OpenAIPromptExecutionSettings
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(settings));
        return Ok(new { response = result.ToString() });
    }
}
```

**Probar desde Swagger:**
- `POST /api/kernel/prompt` → `{ "prompt": "Escribe un poema corto sobre Semantic Kernel" }`
- `POST /api/kernel/prompt/configurado` → `{ "prompt": "3 ideas de negocio con IA", "maxTokens": 100, "temperature": 0.9 }`

**🔗 Conexión con el Proyecto Real:**
Se muestra la configuración del Kernel en el sistema de producción del laboratorio clínico, con soporte multi-LLM.

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Program.cs` | Startup con Kernel + Swagger |
| `appsettings.json` | Configuración LLM |
| `Controllers/KernelController.cs` | Endpoints de prompt |
| `DTOs/Requests.cs` | Records `PromptRequest`, `PromptConSettingsRequest` |

---

---

### 📅 SESIÓN 2 — Servicios Multimodales: Streaming, Audio e Imágenes (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Chat Completion, Streaming SSE, DALL-E, Text-to-Speech, Whisper |
| **Subtemas** | `IChatCompletionService`, streaming token-by-token, servicios multimodales, `OpenAIPromptExecutionSettings`, pragma experimental |
| **Herramientas** | Semantic Kernel, ASP.NET Core, Swagger, OpenAI API (DALL-E 3, TTS, Whisper), Azure OpenAI |
| **Rama Git** | `sesion/02` |
| **Script Azure** | `02-crear-deployment-whisper.ps1` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

- **Chat Completion**: `InvokePromptAsync()` vs `GetChatMessageContentAsync()`
- **Streaming**: `GetStreamingChatMessageContentsAsync()` → Server-Sent Events (SSE), respuesta token por token
- **DALL-E**: `AddOpenAITextToImage()`, `ITextToImageService`, configuración de tamaño/calidad/estilo
- **Text-to-Speech**: `AddOpenAITextToAudio()`, modelos de voz (alloy, echo, fable, onyx, nova, shimmer)
- **Whisper (Audio-to-Text)**: `AddOpenAIAudioToText()`, límite de 25MB
- **Pragma experimental**: `#pragma warning disable SKEXP0010` — servicios multimedia están en preview

---

#### Bloque Azure Setup (15 min)

**🔧 Crear deployment de Whisper (audio):**

**Portal de Azure / Foundry:**
1. Ir al recurso Azure OpenAI → **Deployments** → **+ Deploy model**
2. Seleccionar `gpt-4o-mini-transcribe` (o `whisper`)
3. Si el modelo no está disponible en su región → crear recurso separado en región compatible
4. **Deploy** → esperar `Succeeded`

**Azure CLI:**
```powershell
cd Scripts/Azure
.\02-crear-deployment-whisper.ps1
```

---

#### Bloque Práctico (2h)

**Ejercicio 2.1 — MultimodalController con Streaming SSE**

```csharp
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

**Ejercicio 2.2 — Registrar servicios multimodales en Program.cs**

```csharp
#pragma warning disable SKEXP0010
kernelBuilder.AddOpenAITextToImage(apiKey: "...", modelId: "dall-e-3");
kernelBuilder.AddOpenAITextToAudio(apiKey: "...", modelId: "tts-1");
kernelBuilder.AddOpenAIAudioToText(apiKey: "...", modelId: "whisper-1");
#pragma warning restore SKEXP0010
```

**Probar desde Swagger:**
- `POST /api/multimodal/stream` → streaming token por token

**🔗 Conexión con el Proyecto Real:**
Se muestra `CotizacionIntegration.TranscribeAudio()` — transcripción de notas de voz de WhatsApp usando Azure OpenAI.

---

**Archivos creados/modificados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Controllers/MultimodalController.cs` | Streaming SSE |
| `Program.cs` | + servicios multimodales |

---

---

### 📅 SESIÓN 3 — Workshop: Blog Generator + Chat History (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Proyecto Blog con Gutenberg + Gestión de sesiones de Chat |
| **Subtemas** | Bloques Gutenberg, WordPress REST API, ChatHistory roles, ConcurrentDictionary, patrón Service Layer |
| **Herramientas** | Semantic Kernel, ASP.NET Core, DALL-E 3, TTS, WordPress API, ChatHistory |
| **Rama Git** | `sesion/03` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (30 min)

- **Bloques Gutenberg**: Estructura HTML con comentarios `<!-- wp:heading -->`, etc.
- **WordPress REST API**: Autenticación con Application Passwords, crear posts con contenido HTML
- **Patrón Service Layer**: Lógica de negocio en servicios inyectados, no en controladores
- **ChatHistory**: Tres roles — `System` (comportamiento), `User` (mensajes), `Assistant` (respuestas)
- **Gestión de sesiones**: `ConcurrentDictionary<string, ChatHistory>` por sessionId

---

#### Bloque Práctico (2h 30min)

**Ejercicio 3.1 — BlogService + BlogController**

```csharp
// Services/BlogService.cs
public class BlogService
{
    private readonly Kernel _kernel;
    public BlogService(Kernel kernel) => _kernel = kernel;

    public async Task<string> GenerarContenidoBlog(string tema)
    {
        var blogPrompt = $"""
            Genera una publicación de blog sobre {tema}.
            Es OBLIGATORIO usar bloques Gutenberg:
            <!-- wp:heading --><h2 class="wp-block-heading">TEXTO</h2><!-- /wp:heading -->
            <!-- wp:paragraph --><p>TEXTO</p><!-- /wp:paragraph -->
            """;
        var result = await _kernel.InvokePromptAsync(blogPrompt);
        return result.ToString();
    }
}
```

```csharp
// Controllers/BlogController.cs
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
        return Ok(new { contenidoHtml = contenido });
    }
}
```

**Ejercicio 3.2 — ChatSessionService + ChatController**

```csharp
// Services/ChatSessionService.cs
public class ChatSessionService
{
    private readonly ConcurrentDictionary<string, ChatHistory> _sessions = new();
    private readonly Kernel _kernel;

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

```csharp
// Controllers/ChatController.cs
[ApiController]
[Route("api/[controller]")]
[Tags("4️⃣ Chat — Sesiones 3-4")]
public class ChatController : ControllerBase
{
    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request) { ... }

    [HttpGet("{sessionId}/historial")]
    public IActionResult ObtenerHistorial(string sessionId) { ... }

    [HttpGet("sesiones")]
    public IActionResult ListarSesiones() { ... }

    [HttpDelete("{sessionId}")]
    public IActionResult EliminarSesion(string sessionId) { ... }
}
```

**Probar desde Swagger:**
- `POST /api/blog/generar` → `{ "tema": "Cómo usar Semantic Kernel en .NET" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "Mi nombre es José" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }` → ¡Recuerda el contexto!

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo `ConversationSession` mantiene estado por número de teléfono en el agente del laboratorio.

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Services/BlogService.cs` | Generación de blog HTML Gutenberg |
| `Controllers/BlogController.cs` | Endpoint `/generar` |
| `Services/ChatSessionService.cs` | Gestión de sesiones con ChatHistory |
| `Controllers/ChatController.cs` | Endpoints de chat por sesión |
| `DTOs/Requests.cs` | + `BlogRequest`, `ChatMensajeRequest` |

---

---

### 📅 SESIÓN 4 — Chat Multimodal + Persistencia EF Core (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Chat con imágenes + Entity Framework Core + SQLite |
| **Subtemas** | `ImageContent`, `ChatMessageContentItemCollection`, EF Core Code First, Migrations, Seed Data, modelos de datos |
| **Herramientas** | Semantic Kernel, EF Core 9.0.4, SQLite, ASP.NET Core |
| **Rama Git** | `sesion/04` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

- **Chat Multimodal**: `ChatMessageContentItemCollection` para enviar texto + imágenes
- **ImageContent**: Soporta URLs (`new ImageContent(new Uri(...))`) y bytes locales
- **GPT-4 Vision**: Análisis de imágenes con prompts descriptivos
- **EF Core + SQLite**: Code First, `DbContext`, `DbSet<T>`, Migrations, Seed Data
- **Modelos**: `ChatSession`, `ChatMessage`, `PluginInvocationLog`, `ContenidoGenerado`

---

#### Bloque Práctico (2h 15min)

**Ejercicio 4.1 — Agregar endpoints de imagen al ChatController**

```csharp
[HttpPost("{sessionId}/imagen")]
public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
{
    var contents = new ChatMessageContentItemCollection();
    contents.Add(new TextContent(request.Pregunta ?? "Describe esta imagen"));
    contents.Add(new ImageContent(new Uri(request.ImagenUrl)));
    // enviar al chat service y devolver respuesta
}
```

**Ejercicio 4.2 — Crear modelos EF Core y DbContext**

```csharp
// Models/ChatModels.cs
public class ChatSession { ... }
public class ChatMessage { ... }
public class PluginInvocationLog { ... }
public class ContenidoGenerado { ... }

// Data/AppDbContext.cs
public class AppDbContext : DbContext
{
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<PluginInvocationLog> PluginLogs => Set<PluginInvocationLog>();
    public DbSet<ContenidoGenerado> ContenidosGenerados => Set<ContenidoGenerado>();
}
```

**Ejercicio 4.3 — Registrar EF Core en Program.cs**

```csharp
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));
```

**Probar desde Swagger:**
- `POST /api/chat/session1/imagen` → `{ "imagenUrl": "https://...", "pregunta": "¿Qué ves?" }`
- `GET /api/chat/session1/historial` → ver mensajes con imágenes

---

**Archivos creados/modificados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Models/ChatModels.cs` | Entidades EF Core (ChatSession, ChatMessage, PluginInvocationLog, ContenidoGenerado) |
| `Data/AppDbContext.cs` | DbContext + Seed Data |
| `Controllers/ChatController.cs` | + endpoints de imagen |
| `DTOs/Requests.cs` | + `ChatImagenRequest` |
| `Program.cs` | + EF Core SQLite |

---

---

### 📅 SESIÓN 5 — Plugins y Function Calling (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Plugins nativos en C# + Function Calling automático |
| **Subtemas** | `[KernelFunction]`, `[Description]`, plugins nativos, plugins pre-construidos (TimePlugin), `FunctionChoiceBehavior.Auto()`, registro de plugins en Kernel |
| **Herramientas** | Semantic Kernel Plugins, Function Calling, ASP.NET Core |
| **Rama Git** | `sesion/05` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (1h)

**¿Qué son los Plugins?**
- Funciones nativas en C# decoradas con `[KernelFunction]` y `[Description]`
- El LLM "ve" las descripciones y decide cuándo invocar cada función
- Son el puente entre la IA y la lógica de negocio real

**Atributos clave:**
```csharp
[KernelFunction("nombre_funcion")]
[Description("Descripción detallada")]
public async Task<string> MiFuncion(
    [Description("Descripción del parámetro")] string parametro) { }
```

**Function Calling:**
- `FunctionChoiceBehavior.Auto()` → el LLM decide automáticamente qué funciones llamar
- `FunctionChoiceBehavior.Required()` → el LLM DEBE invocar al menos una función

**Registro de plugins:**
```csharp
// Con dependencias inyectadas:
kernel.ImportPluginFromObject(new ClimaPlugin(), "Clima");
// Sin dependencias (SK crea la instancia):
kernelBuilder.Plugins.AddFromType<MathPlugin>("Matematica");
```

---

#### Bloque Práctico (2h)

**Ejercicio 5.1 — ClimaPlugin + MathPlugin**

```csharp
// Plugins/ClimaPlugin.cs
public class ClimaPlugin
{
    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad de Honduras")]
    public string ObtenerClima([Description("Nombre de la ciudad")] string ciudad) =>
        _climas.GetValueOrDefault(ciudad, $"No tengo datos para {ciudad}");

    [KernelFunction("obtener_fecha_hora")]
    [Description("Obtiene la fecha y hora actual del servidor")]
    public string ObtenerFechaHora() =>
        $"Fecha: {DateTime.Now:dd/MM/yyyy}, Hora: {DateTime.Now:hh:mm tt}";
}

// Plugins/MathPlugin.cs
public class MathPlugin
{
    [KernelFunction("sumar")] [Description("Suma dos números")]
    public double Sumar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a + b;
    // + restar, multiplicar, dividir
}
```

**Ejercicio 5.2 — AgentController con Function Calling**

```csharp
// Controllers/AgentController.cs
[ApiController]
[Route("api/[controller]")]
[Tags("5️⃣ Agent — Sesiones 5-6")]
public class AgentController : ControllerBase
{
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
    public IActionResult ListarPlugins() { ... }
}
```

> **Demostrar Function Calling:**
> - `"¿Cuál es el clima en Tegucigalpa?"` → invoca `obtener_clima("Tegucigalpa")`
> - `"¿Cuánto es 15 × 7 + 3?"` → invoca `multiplicar(15, 7)` → `sumar(105, 3)`

**🔗 Conexión con el Proyecto Real:**
Se estudian los 6 plugins del agente del laboratorio, especialmente `CotizacionPlugin.buscar_cliente_identidad()` y `CitaPlugin.agendar_cita()`.

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Plugins/ClimaPlugin.cs` | Plugin de clima con datos simulados |
| `Plugins/MathPlugin.cs` | Plugin de operaciones matemáticas |
| `Controllers/AgentController.cs` | Endpoint con Function Calling automático |
| `Program.cs` | + registro de plugins |

---

---

### 📅 SESIÓN 6 — Plugins Avanzados + Filtros + OpenAPI (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Plugins avanzados, DI en plugins, FunctionInvocationFilter, OpenAPI plugins, logging y auditoría |
| **Subtemas** | `AddFromObject` vs `AddFromType`, inyección de `HttpClient` en plugins, interceptores, plugins externos OpenAPI |
| **Herramientas** | Semantic Kernel Filters, HttpClient, OpenAPI, ASP.NET Core |
| **Rama Git** | `sesion/06` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (1h)

**AddFromObject vs AddFromType:**

| Método | Cuándo usar | Constructor |
|---|---|---|
| `ImportPluginFromObject(instance)` | Plugin necesita dependencias (HttpClient, DbContext, ILogger) | Manual — tú creas la instancia |
| `AddFromType<T>()` | Plugin sin dependencias (métodos estáticos o simples) | SK crea la instancia automáticamente |

**FunctionInvocationFilter:**
- Intercepta TODAS las invocaciones de funciones del Kernel
- Implementa `IFunctionInvocationFilter` con método `OnFunctionInvocationAsync()`
- Ideal para: logging, auditoría, control de acceso, métricas

**Plugins OpenAPI:**
- `ImportPluginFromOpenApiAsync()` genera plugin desde una especificación OpenAPI/Swagger

---

#### Bloque Práctico (2h)

**Ejercicio 6.1 — LoggingFilter**

```csharp
// Filters/LoggingFilter.cs
public class LoggingFilter : IFunctionInvocationFilter
{
    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        _logger.LogInformation("▶ Invocando {Plugin}.{Function}", context.Function.PluginName, context.Function.Name);
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();
        _logger.LogInformation("✔ Completado en {Ms}ms → {Result}", sw.ElapsedMilliseconds, context.Result);
    }
}
```

```csharp
// Registrar en Program.cs:
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, LoggingFilter>();
```

**Ejercicio 6.2 — Plugin con HttpClient inyectado (ejemplo)**

```csharp
public class ApiExternaPlugin
{
    private readonly HttpClient _http;
    public ApiExternaPlugin(HttpClient http) => _http = http;

    [KernelFunction("consultar_api")]
    [Description("Consulta una API externa")]
    public async Task<string> ConsultarApi([Description("URL a consultar")] string url)
    {
        var response = await _http.GetStringAsync(url);
        return response;
    }
}

// Registro con ImportPluginFromObject:
kernel.ImportPluginFromObject(new ApiExternaPlugin(httpClient), "ApiExterna");
```

**Ejercicio 6.3 — Plugin OpenAPI (demostración)**

```csharp
await kernel.ImportPluginFromOpenApiAsync("WeatherAPI",
    new Uri("https://api.weather.gov/openapi.json"));
```

**🔗 Conexión con el Proyecto Real:**
Se estudia cómo los plugins del laboratorio interactúan con APIs externas y cómo se podría agregar auditoría con filtros.

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Filters/LoggingFilter.cs` | Interceptor de invocaciones con Stopwatch |
| `Program.cs` | + registro del filtro |

---

---

### 📅 SESIÓN 7 — Prompting Avanzado + Templates YAML (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Técnicas de prompting, plantillas Handlebars/Liquid/YAML, archivos de template separados |
| **Subtemas** | Zero-Shot, Few-Shot, Chain of Thought, Generated Knowledge, `KernelFunctionYaml`, `PromptTemplateConfig`, variables en templates |
| **Herramientas** | Semantic Kernel Templates, YAML, Handlebars, Liquid |
| **Rama Git** | `sesion/07` |
| **Script Azure** | `07-crear-deployment-embedding.ps1` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

**Técnicas de Prompting:**

| Técnica | Descripción | Ejemplo |
|---|---|---|
| **Zero-Shot** | Instrucción directa sin ejemplos | "Clasifica el sentimiento: Positivo, Negativo, Neutro" |
| **Few-Shot** | Incluir ejemplos en el prompt | "Ejemplo: 'Me encanta' → Positivo. Ahora clasifica: ..." |
| **Chain of Thought** | Pedir razonamiento paso a paso | "Piensa paso a paso antes de responder..." |
| **Generated Knowledge** | El LLM genera conocimiento primero, luego responde | "Primero genera 3 hechos relevantes, luego responde..." |

**Formatos de Templates en SK:**

| Formato | Sintaxis | Ejemplo |
|---|---|---|
| SK Nativo | `{{$variable}}` | `Hola {{$nombre}}` |
| Handlebars | `{{variable}}` | `Clasifica: {{input}}` |
| YAML | Archivo `.yaml` con config + template | Separar config de template |

---

#### Bloque Azure Setup (15 min)

**🔧 Crear deployment de embeddings (preparación para sesión 8):**

```powershell
cd Scripts/Azure
.\07-crear-deployment-embedding.ps1
```

---

#### Bloque Práctico (2h)

**Ejercicio 7.1 — PromptingController con 4 técnicas**

```csharp
[ApiController]
[Route("api/[controller]")]
[Tags("7️⃣ Prompting — Sesión 7")]
public class PromptingController : ControllerBase
{
    [HttpPost("zero-shot")]
    public async Task<IActionResult> ZeroShot([FromBody] PromptRequest request) { ... }

    [HttpPost("few-shot")]
    public async Task<IActionResult> FewShot([FromBody] PromptRequest request) { ... }

    [HttpPost("chain-of-thought")]
    public async Task<IActionResult> ChainOfThought([FromBody] PromptRequest request) { ... }

    [HttpPost("yaml")]
    public async Task<IActionResult> DesdeYaml([FromBody] PromptRequest request)
    {
        var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "ClasificarIntencion.yaml");
        var yaml = await System.IO.File.ReadAllTextAsync(yamlPath);
        var config = KernelFunctionYaml.ToPromptTemplateConfig(yaml);
        var factory = new HandlebarsPromptTemplateFactory();
        var function = KernelFunctionFactory.CreateFromPrompt(config, factory);
        var result = await _kernel.InvokeAsync(function, new() { ["consulta"] = request.Prompt });
        return Ok(new { tecnica = "YAML Template", resultado = result.ToString() });
    }
}
```

**Ejercicio 7.2 — Archivo Prompts/ClasificarIntencion.yaml**

```yaml
name: ClasificarIntencion
template_format: handlebars
template: |
  Clasifica la intención del usuario en UNA categoría:
  - consulta_general, solicitud_prestamo, estado_solicitud,
    consulta_legal, simulacion, reclamo
  Consulta: {{consulta}}
  Categoría:
input_variables:
  - name: consulta
    is_required: true
execution_settings:
  default:
    max_tokens: 20
    temperature: 0.0
```

> Agregar al `.csproj`: `<None Update="Prompts\**\*.yaml"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory></None>`

**Probar desde Swagger:**
- Comparar Zero-Shot vs Few-Shot vs Chain-of-Thought con el mismo texto
- `POST /api/prompting/yaml` → clasificación con template YAML

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Controllers/PromptingController.cs` | 4 técnicas de prompting |
| `Prompts/ClasificarIntencion.yaml` | Template YAML Handlebars |

---

---

### 📅 SESIÓN 8 — Workshop Podcast + Intro Embeddings (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Pipeline de generación de contenido + Fundamentos de Embeddings |
| **Subtemas** | Pipeline datos→markdown→brainstorm→script→audio, embeddings como vectores, modelos de embedding, similaridad coseno |
| **Herramientas** | Semantic Kernel, TTS, Embeddings (text-embedding-3-small), System.Numerics.Tensors |
| **Rama Git** | `sesion/08` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

**Pipeline de Generación de Podcast:**
1. **Recolección de datos**: URLs, texto, tema
2. **Conversión a Markdown**: Limpiar HTML → texto estructurado
3. **Brainstorm**: LLM genera lluvia de ideas/puntos clave
4. **Script**: LLM escribe el guion del podcast
5. **Audio**: TTS sintetiza el audio final

**Fundamentos de Embeddings:**
- **¿Qué es un embedding?** Representación numérica del significado de un texto
- Vector de 1536 dimensiones (text-embedding-3-small)
- **Similaridad coseno**: mide qué tan "parecidos" son dos textos

```
"préstamo hipotecario" → [0.023, -0.041, 0.187, ...]  (1536 dims)
"crédito para vivienda" → [0.019, -0.038, 0.192, ...]  (1536 dims)
CosineSimilarity ≈ 0.95 → ¡Son semánticamente similares!
```

---

#### Bloque Práctico (2h 15min)

**Ejercicio 8.1 — VectorStoreService + DocumentoVectorial**

```csharp
// Models/DocumentoVectorial.cs
public class DocumentoVectorial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public ReadOnlyMemory<float>? Embedding { get; set; }
}

// Services/VectorStoreService.cs
public class VectorStoreService
{
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _store = new();
    private readonly ITextEmbeddingGenerationService? _embeddingService;

    public async Task<string> IndexarDocumento(string titulo, string contenido, string categoria, string? fuente = null)
    {
        var embedding = await _embeddingService!.GenerateEmbeddingAsync(contenido);
        var doc = new DocumentoVectorial { Titulo = titulo, Contenido = contenido, Categoria = categoria, Embedding = embedding };
        _store.TryAdd(doc.Id, doc);
        return doc.Id;
    }

    public async Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilares(string consulta, int top = 3)
    {
        var queryEmbedding = await _embeddingService!.GenerateEmbeddingAsync(consulta);
        return _store.Values
            .Where(d => d.Embedding.HasValue)
            .Select(d => (Doc: d, Score: (double)TensorPrimitives.CosineSimilarity(
                queryEmbedding.Span, d.Embedding!.Value.Span)))
            .OrderByDescending(x => x.Score)
            .Take(top).ToList();
    }
}
```

**Ejercicio 8.2 — Registrar servicio de embeddings en Program.cs**

```csharp
kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: builder.Configuration["LLMSettings:Embedding:DeploymentName"]!,
    endpoint: builder.Configuration["LLMSettings:Embedding:Endpoint"]!,
    apiKey: builder.Configuration["LLMSettings:Embedding:ApiKey"]!);

builder.Services.AddSingleton<VectorStoreService>();
```

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Services/VectorStoreService.cs` | Vector store in-memory con CosineSimilarity |
| `Models/DocumentoVectorial.cs` | Modelo de documento vectorial |
| `Program.cs` | + servicio de embeddings + VectorStoreService |

---

---

### 📅 SESIÓN 9 — RAG Completo + Agent Framework (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Retrieval-Augmented Generation + SK Agent Framework |
| **Subtemas** | Patrón RAG (Query→Embed→Search→Context→LLM), indexación de documentos, búsqueda vectorial, `ChatCompletionAgent`, Azure AI Search (opcional) |
| **Herramientas** | Semantic Kernel, VectorStoreService, Azure AI Search (opcional), Agent Framework |
| **Rama Git** | `sesion/09` |
| **Script Azure** | `09-crear-ai-search.ps1` (opcional) |
| **Duración** | 3 horas |

---

#### Bloque Teórico (1h)

**Patrón RAG:**
1. **Retrieve** — Buscar fragmentos relevantes en el vector store
2. **Augment** — Inyectar los fragmentos como contexto del prompt
3. **Generate** — El LLM responde basándose SOLO en el contexto proporcionado

**Ventajas de RAG:**
- Reduce alucinaciones (el LLM se basa en datos reales)
- Conocimiento actualizable sin re-entrenar el modelo
- Reduce costos (menos tokens consumidos vs inyectar todo)

| Enfoque | Cuándo usar | Costo |
|---|---|---|
| Prompt directo | Datos pequeños (<1000 tokens) | Bajo |
| RAG con Vector Store | Datos medianos-grandes, actualizables | Medio |
| Fine-Tuning | Cambiar comportamiento del modelo | Alto |

**RAG en Foundry vs RAG en código:**
- **Foundry**: subir archivos → índice vectorial automático → Playground consulta → citaciones
- **Código (SK)**: `VectorStoreService` → `IndexarDocumento()` → `BuscarSimilares()` → inyectar contexto
- El código da control total; Foundry es rápido para prototipos

**Agent Framework (intro):**
- `ChatCompletionAgent`: agente con Kernel + plugins + instrucciones
- Futuro: `OpenAIAssistantAgent`, orquestación multi-agente

---

#### Bloque Práctico (2h)

**Ejercicio 9.1 — RAGController**

```csharp
[ApiController]
[Route("api/[controller]")]
[Tags("9️⃣ RAG — Sesiones 8-9")]
public class RAGController : ControllerBase
{
    private readonly VectorStoreService _vectorStore;
    private readonly Kernel _kernel;

    [HttpPost("indexar")]
    public async Task<IActionResult> Indexar([FromBody] IndexarDocumentoRequest request) { ... }

    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BusquedaSemanticaRequest request) { ... }

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
    public async Task<IActionResult> SeedFAQs() { ... }
}
```

**Ejercicio 9.2 — Demo Foundry RAG (sin código)**

1. Ir a https://ai.azure.com → Playground
2. Adjuntar archivos (PDF, TXT) → Foundry crea índice vectorial automáticamente
3. Hacer preguntas → observar citaciones del índice
4. Comparar con `POST /api/rag/consultar`

**Probar desde Swagger:**
1. `POST /api/rag/seed` → cargar FAQs de ejemplo
2. `POST /api/rag/buscar` → `{ "consulta": "¿Qué es Semantic Kernel?", "top": 3 }`
3. `POST /api/rag/consultar` → `{ "consulta": "¿Qué es RAG y para qué sirve?" }`

---

**Archivos creados esta sesión:**

| Archivo | Descripción |
|---|---|
| `Controllers/RAGController.cs` | Endpoints RAG (indexar, buscar, consultar, seed) |
| `DTOs/Requests.cs` | + `IndexarDocumentoRequest`, `BusquedaSemanticaRequest` |

---

---

### 📅 SESIÓN 10 — Deployment Azure + Foundry + Integración Final (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Despliegue en Azure App Service + Microsoft Foundry |
| **Subtemas** | `dotnet publish`, zip deploy, variables de entorno, Foundry Playground, guardrails, Agent Service, demo end-to-end |
| **Herramientas** | Azure CLI, App Service, Microsoft Foundry, PowerShell |
| **Rama Git** | `sesion/10` (merge a `main`) |
| **Scripts Azure** | `10-deploy-app-service.ps1`, `10-foundry-setup.ps1` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

**Azure App Service:**
- PaaS de Azure para aplicaciones web .NET
- `dotnet publish -c Release` → zip → `az webapp deploy --type zip`
- Variables de entorno para producción (secrets, connection strings)

**Microsoft Foundry:**
- **Playground**: pruebas interactivas con modelos desplegados
- **File Upload**: subir archivos → chunking → embedding → índice vectorial automático
- **Guardrails**: Content Safety (odio, autolesiones, sexual, violencia), prompt injection filter
- **Agent Service**: 3 tipos
  - Agentes de Solicitud (sin código, portal)
  - Agentes de Flujo de Trabajo (YAML, multi-agente)
  - Agentes Hospedados (contenedor, SK/LangGraph)

---

#### Bloque Práctico (2h 15min)

**Ejercicio 10.1 — Deploy a Azure App Service**

```powershell
cd Scripts/Azure
.\10-deploy-app-service.ps1
```

El script:
1. Ejecuta `dotnet publish -c Release`
2. Crea zip del output
3. Crea App Service Plan + Web App
4. Configura variables de entorno (LLMSettings)
5. Ejecuta `az webapp deploy --type zip`

**Ejercicio 10.2 — Configurar Microsoft Foundry**

Seguir: `Scripts/Azure/10-foundry-setup.ps1`

**Ejercicio 10.3 — Demo end-to-end**

1. Abrir Swagger en la URL pública de Azure
2. Probar todos los endpoints (sesiones 1-9)
3. Foundry: subir documentos → crear índice → consultar RAG desde portal
4. Comparar RAG en Foundry vs RAG en código

**Ejercicio 10.4 — Merge a main**

```bash
git checkout main
git merge sesion/10
git tag v1.0.0 -m "Curso completo - 10 sesiones"
git push origin main --tags
```

---

**Archivos creados/modificados esta sesión:**

| Archivo | Descripción |
|---|---|
| `appsettings.Production.json` | Configuración para producción |

---

---

## Resumen de Archivos por Sesión

| Sesión | Archivos Nuevos/Modificados |
|---|---|
| **1** | `Program.cs`, `appsettings.json`, `KernelController.cs`, `Requests.cs` |
| **2** | `MultimodalController.cs`, `Program.cs` (+ servicios multimodales) |
| **3** | `BlogService.cs`, `BlogController.cs`, `ChatSessionService.cs`, `ChatController.cs`, `Requests.cs` (+ DTOs) |
| **4** | `ChatModels.cs`, `AppDbContext.cs`, `ChatController.cs` (+ imagen), `Requests.cs` (+ ChatImagenRequest), `Program.cs` (+ EF Core) |
| **5** | `ClimaPlugin.cs`, `MathPlugin.cs`, `AgentController.cs`, `Program.cs` (+ plugins) |
| **6** | `LoggingFilter.cs`, `Program.cs` (+ filtro) |
| **7** | `PromptingController.cs`, `ClasificarIntencion.yaml` |
| **8** | `DocumentoVectorial.cs`, `VectorStoreService.cs`, `Program.cs` (+ embeddings) |
| **9** | `RAGController.cs`, `Requests.cs` (+ DTOs RAG) |
| **10** | `appsettings.Production.json` (deploy) |

---

## Evaluación

| Criterio | Peso |
|---|---|
| Participación y ejercicios en clase | 30% |
| Proyecto final: API desplegada con todos los módulos | 40% |
| Quiz técnico (sesiones 5 y 10) | 20% |
| Documentación del proyecto | 10% |

---

## Recursos Adicionales

- [Semantic Kernel — Documentación oficial](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
- [Microsoft Foundry](https://learn.microsoft.com/es-mx/azure/foundry/)
- [Foundry Agent Service](https://learn.microsoft.com/es-mx/azure/foundry/agents/overview)
- [SK Samples — GitHub](https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples)
