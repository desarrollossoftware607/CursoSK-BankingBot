# Curso Unificado: Semantic Kernel + Agentes de IA con Azure OpenAI

> **Este documento unifica y reemplaza a `CURSO_SEMANTIC_KERNEL_JORNALIZACION.md` y `CURSO_AGENTES_IA_AZURE_JORNALIZACION.md`.**

---

## Información General

| Campo | Detalle |
|-------|---------|
| **Nombre del Curso** | Semantic Kernel + Agentes de IA con Azure OpenAI |
| **Duración** | 30 horas cronológicas (10 sesiones × 3 horas) |
| **Horario** | Lunes, Miércoles y Viernes — 3 horas por sesión |
| **Modalidad** | Formación práctica de alto nivel — "Aprender Haciendo" |
| **Requisitos** | Conocimientos básicos de C# / .NET, Visual Studio 2022 o VS Code con extensión C#, suscripción Azure (crédito $200 nuevos usuarios) |
| **Proyectos** | **CursoSK.Api** (API genérica de IA) + **CursoSK.BankingBot** (Bot bancario especializado) |
| **Repositorio** | Monorepo con ramas Git por sesión (`sesion/01` a `sesion/10`) |
| **Idioma** | Español |

---

## Objetivo General

Al finalizar el curso, los participantes serán capaces de **construir APIs REST inteligentes con ASP.NET Core y Semantic Kernel** que integren modelos de IA (Azure OpenAI) para: generar texto, imágenes y audio; crear plugins nativos con Function Calling; implementar técnicas avanzadas de prompting; utilizar Vector Stores para búsqueda semántica (RAG); y desplegar agentes de IA en producción con Azure App Service y Microsoft Foundry — todo desarrollado sobre **dos proyectos Web API que crecen sesión a sesión**.

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
| 11 | Desplegar APIs en Azure App Service y configurar Microsoft Foundry | 10 |

---

## Proyectos del Curso

### CursoSK.Api — API Genérica de IA (Puerto 5192)

Proyecto genérico que implementa los conceptos de Semantic Kernel de forma progresiva:

| Módulo | Controller | Endpoints | Sesión |
|---|---|---|---|
| Kernel Core | `KernelController` | `/api/kernel/prompt`, `/prompt/configurado` | 1 |
| Multimodal | `MultimodalController` | `/api/multimodal/stream` (SSE) | 2 |
| Blog Generator | `BlogController` | `/api/blog/generar` | 3 |
| Chat | `ChatController` | `/api/chat/{id}/mensaje`, `/imagen`, `/historial` | 3-4 |
| Function Calling | `AgentController` | `/api/agent/consultar`, `/plugins` | 5-6 |
| Prompting | `PromptingController` | `/api/prompting/zero-shot`, `/few-shot`, `/chain-of-thought`, `/yaml` | 7 |
| RAG | `RAGController` | `/api/rag/indexar`, `/buscar`, `/consultar`, `/seed` | 8-9 |

### CursoSK.BankingBot — Bot Bancario (Puerto 5290)

Proyecto especializado en dominio bancario con caso de uso real:

| Módulo | Controller | Endpoints | Sesión |
|---|---|---|---|
| Onboarding | `OnboardingController` | `/api/onboarding/iniciar` | 5 |
| Chat Bancario | `ChatController` | `/api/chat/mensaje` | 3 |
| Legal | `LegalController` | `/api/legal/consultar`, `/consultar/rag` | 6, 9 |
| Audio | `AudioController` | `/api/audio/transcribir` | 2 |
| RAG Legal | `RAGController` | `/api/rag/indexar/leyes`, `/buscar` | 9 |
| Préstamos | `PrestamosController` | `/api/prestamos/simular` | 6 |

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

| Rama | Contenido Acumulado |
|---|---|
| `sesion/01` | Setup de ambos proyectos + `KernelController` + scripts Azure básicos |
| `sesion/02` | + `MultimodalController` + `AudioController` + deployment Whisper |
| `sesion/03` | + `BlogController` + `ChatController` (texto) + services |
| `sesion/04` | + Chat multimodal con imágenes + EF Core + modelos + seed data |
| `sesion/05` | + `ClimaPlugin` + `MathPlugin` + `OnboardingPlugin` + `AgentController` |
| `sesion/06` | + `LoggingFilter` + `LegalPlugin` + `PrestamosController` + plugins avanzados |
| `sesion/07` | + `PromptingController` + templates YAML/Handlebars + deployment embeddings |
| `sesion/08` | + Intro embeddings + `VectorStoreService` + `DocumentoVectorial` |
| `sesion/09` | + `RAGController` + indexación de leyes + búsqueda vectorial + Agent Framework |
| `sesion/10` | + Deploy App Service + Foundry config + producción |
| `main` | Estado final completo (merge de sesion/10) |

```bash
# Cambiar a la rama de una sesión específica
git checkout sesion/05
dotnet build CursoSK.Api/CursoSK.Api.csproj
dotnet build CursoSK.BankingBot/CursoSK.BankingBot.csproj
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
# Ejecutar el script incluido en el repositorio:
cd Scripts/Azure
. .\00-variables.ps1          # Cargar variables compartidas
.\01-crear-recurso-openai.ps1 # Crear RG + OpenAI + deployment

# O manualmente:
az login
az group create --name rg-cursosk --location eastus
az cognitiveservices account create --name cursosk-openai --resource-group rg-cursosk --kind OpenAI --sku s0 --location eastus
az cognitiveservices account deployment create --name cursosk-openai --resource-group rg-cursosk --deployment-name gpt-35-turbo-16k --model-name gpt-35-turbo-16k --model-version "0613" --model-format OpenAI --sku-capacity 10 --sku-name Standard
```

> ⚠️ **IMPORTANTE:** El `deploymentName` es lo que se usa en las llamadas API, NO el nombre del modelo. Azure OpenAI siempre requiere el deployment name.

---

#### Bloque Práctico (1h 30min)

**Ejercicio 1.1 — Crear ambos proyectos Web API + Semantic Kernel**

```bash
# Proyecto 1: CursoSK.Api (genérico)
dotnet new webapi -n CursoSK.Api -controllers
cd CursoSK.Api
dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.4

# Proyecto 2: CursoSK.BankingBot (bancario)
cd ..
dotnet new webapi -n CursoSK.BankingBot -controllers
cd CursoSK.BankingBot
dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
```

**Ejercicio 1.2 — Configurar Kernel en Program.cs (CursoSK.Api)**

```csharp
// Program.cs — Inyección del Kernel en ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Semantic Kernel
var llmProvider = builder.Configuration["LLMSettings:Provider"]?.ToLower() ?? "AzureOpenAI";
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
        deploymentName: builder.Configuration["LLMSettings:AzureOpenAI:ChatDeployment"]!,
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
    "Provider": "AzureOpenAI",
    "AzureOpenAI": {
      "Endpoint": "https://TU-RECURSO.openai.azure.com/",
      "ApiKey": "TU-API-KEY",
      "ChatDeployment": "gpt-35-turbo-16k"
    }
  }
}
```

**Ejercicio 1.3 — Primer Controller: KernelController**

```csharp
// Controllers/KernelController.cs
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
```

**Probar desde Swagger:**
- `POST /api/kernel/prompt` → `{ "prompt": "Escribe un poema corto sobre Semantic Kernel" }`
- `POST /api/kernel/prompt/configurado` → `{ "prompt": "3 ideas de negocio con IA", "maxTokens": 100, "temperature": 0.9 }`

**BankingBot — Setup equivalente:**
- Configurar `Program.cs` con Kernel apuntando a Azure OpenAI
- Estructura base del proyecto con carpetas Controllers, Services, Plugins, Models

**🔗 Conexión con el Proyecto Real:**
Se muestra la configuración del Kernel en el sistema de producción del laboratorio clínico, con soporte multi-LLM (Azure OpenAI + Google Gemini).

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Program.cs` | Startup con Kernel + Swagger |
| CursoSK.Api | `appsettings.json` | Configuración LLM |
| CursoSK.Api | `Controllers/KernelController.cs` | Endpoints de prompt |
| CursoSK.Api | `DTOs/Requests.cs` | Records `PromptRequest`, `PromptConSettingsRequest` |
| CursoSK.BankingBot | `Program.cs` | Startup con Kernel Azure |
| Scripts/Azure | `01-crear-recurso-openai.ps1` | Crear Resource Group + Azure OpenAI |

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
3. Deployment name: `gpt-4o-mini-transcribe`
4. Si el modelo no está disponible en su región → crear recurso separado en región compatible
5. **Deploy** → esperar `Succeeded`

**Azure CLI:**
```powershell
cd Scripts/Azure
.\02-crear-deployment-whisper.ps1
```

> **Nota:** Los modelos de audio pueden requerir un recurso Azure OpenAI separado si la región principal no los soporta. El script maneja este caso automáticamente.

---

#### Bloque Práctico (2h)

**Ejercicio 2.1 — MultimodalController con Streaming SSE (CursoSK.Api)**

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

**Ejercicio 2.2 — Registrar servicios multimodales en Program.cs**

```csharp
// Program.cs — Añadir servicios de imagen, audio y TTS
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

**Ejercicio 2.3 — AudioController (CursoSK.BankingBot)**

```csharp
// Controllers/AudioController.cs — Transcripción de audio bancario
[HttpPost("transcribir")]
public async Task<IActionResult> Transcribir(IFormFile archivo)
{
    // Enviar audio a Azure OpenAI Whisper para transcripción
    // Útil para: transcribir llamadas de servicio al cliente, grabar actas de reuniones
}
```

**Probar desde Swagger:**
- `POST /api/multimodal/stream` → streaming token por token
- `POST /api/audio/transcribir` → subir archivo de audio

**🔗 Conexión con el Proyecto Real:**
Se muestra `CotizacionIntegration.TranscribeAudio()` — transcripción de notas de voz de WhatsApp usando Azure OpenAI `gpt-4o-mini-transcribe`.

---

**Archivos creados/modificados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Controllers/MultimodalController.cs` | Streaming SSE |
| CursoSK.Api | `Program.cs` | Agregar servicios multimodales |
| CursoSK.BankingBot | `Controllers/AudioController.cs` | Transcripción Whisper |

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

- **Bloques Gutenberg**: Estructura HTML con comentarios `<!-- wp:heading -->`, `<!-- wp:paragraph -->`, etc.
- **WordPress REST API**: Autenticación con Application Passwords, crear posts con contenido HTML
- **Patrón Service Layer**: Lógica de negocio en servicios inyectados, no en controladores
- **ChatHistory**: Tres roles — `System` (comportamiento), `User` (mensajes), `Assistant` (respuestas)
- **Gestión de sesiones**: `ConcurrentDictionary<string, ChatHistory>` por sessionId

---

#### Bloque Práctico (2h 30min)

**Ejercicio 3.1 — BlogService + BlogController (CursoSK.Api)**

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
            <!-- wp:list --><ul class="wp-block-list"><li>ITEM</li></ul><!-- /wp:list -->
            <!-- wp:code --><pre class="wp-block-code"><code>CÓDIGO</code></pre><!-- /wp:code -->
            """;
        var result = await _kernel.InvokePromptAsync(blogPrompt);
        return result.ToString();
    }
}
```

```csharp
// Controllers/BlogController.cs
[HttpPost("generar")]
public async Task<IActionResult> GenerarBlogPost([FromBody] BlogRequest request)
{
    var contenido = await _blogService.GenerarContenidoBlog(request.Tema);
    return Ok(new { contenidoHtml = contenido });
}
```

**Ejercicio 3.2 — ChatSessionService + ChatController (CursoSK.Api)**

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
[HttpPost("{sessionId}/mensaje")]
public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
{
    var respuesta = await _chatService.EnviarMensaje(sessionId, request.Mensaje);
    return Ok(new { sessionId, respuesta });
}
```

**Ejercicio 3.3 — ConversationService (CursoSK.BankingBot)**

- Implementar servicio de conversación con system prompt bancario
- Roles: asistente financiero con restricciones de compliance

**Probar desde Swagger:**
- `POST /api/blog/generar` → `{ "tema": "Cómo usar Semantic Kernel en .NET" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "Mi nombre es José" }`
- `POST /api/chat/session1/mensaje` → `{ "mensaje": "¿Cuál es mi nombre?" }` → ¡Recuerda el contexto!

**🔗 Conexión con el Proyecto Real:**
Se muestra cómo `ConversationSession` mantiene estado por número de teléfono en el agente del laboratorio. Los participantes replican esta gestión de estado para el contexto bancario.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Services/BlogService.cs` | Generación de blog HTML Gutenberg |
| CursoSK.Api | `Controllers/BlogController.cs` | Endpoint `/generar` |
| CursoSK.Api | `Services/ChatSessionService.cs` | Gestión de sesiones con ChatHistory |
| CursoSK.Api | `Controllers/ChatController.cs` | Endpoints de chat por sesión |
| CursoSK.BankingBot | `Services/ConversationService.cs` | Conversación bancaria |
| CursoSK.BankingBot | `Controllers/ChatController.cs` | Chat bancario |

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
- **ImageContent**: Soporta URLs (`new ImageContent(new Uri(...))`) y bytes locales (`new ImageContent(bytes, mimeType)`)
- **GPT-4 Vision**: Análisis de imágenes con prompts descriptivos
- **EF Core + SQLite**: Code First, `DbContext`, `DbSet<T>`, Migrations, Seed Data
- **Modelos**: `ChatSession`, `ChatMessage`, `PluginInvocationLog`, `ContenidoGenerado`

---

#### Bloque Práctico (2h 15min)

**Ejercicio 4.1 — Agregar endpoints de imagen al ChatController (CursoSK.Api)**

```csharp
// Endpoint para enviar imagen por URL
[HttpPost("{sessionId}/imagen")]
public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
{
    var contents = new ChatMessageContentItemCollection();
    contents.Add(new TextContent(request.Pregunta ?? "Describe esta imagen"));
    contents.Add(new ImageContent(new Uri(request.ImagenUrl)));
    // ... enviar al chat service y devolver respuesta
}

// Endpoint para subir imagen como archivo
[HttpPost("{sessionId}/imagen/upload")]
public async Task<IActionResult> UploadImagen(string sessionId, IFormFile imagen, [FromForm] string? pregunta)
{
    // Leer bytes de la imagen, crear ImageContent con MIME type inferido
}
```

**Ejercicio 4.2 — Crear modelos EF Core y DbContext (CursoSK.Api)**

```csharp
// Models/ChatModels.cs
public class ChatSession
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public List<ChatMessage> Mensajes { get; set; } = new();
}

public class ChatMessage
{
    public int Id { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
```

```csharp
// Data/AppDbContext.cs
public class AppDbContext : DbContext
{
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<PluginInvocationLog> PluginLogs { get; set; }
    public DbSet<ContenidoGenerado> ContenidosGenerados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data: sesión de ejemplo, mensajes de demo
    }
}
```

**Ejercicio 4.3 — Registrar EF Core en Program.cs**

```csharp
// Program.cs
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));
```

**BankingBot:** Crear modelos bancarios (`ClienteBancario`, `Prestamo`) y `BankingDbContext`.

**Probar desde Swagger:**
- `POST /api/chat/session1/imagen` → `{ "imagenUrl": "https://...", "pregunta": "¿Qué ves?" }`
- `GET /api/chat/session1/historial` → ver mensajes con imágenes
- `GET /api/chat/sesiones` → listar todas las sesiones

---

**Archivos creados/modificados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Models/ChatModels.cs` | Entidades EF Core |
| CursoSK.Api | `Data/AppDbContext.cs` | DbContext + Seed Data |
| CursoSK.Api | `Controllers/ChatController.cs` | + endpoints de imagen |
| CursoSK.Api | `DTOs/Requests.cs` | + `ChatImagenRequest` |
| CursoSK.BankingBot | `Models/*.cs` | Modelos bancarios |
| CursoSK.BankingBot | `Data/BankingDbContext.cs` | DbContext bancario |

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
[KernelFunction("nombre_funcion")]       // Nombre que ve el LLM
[Description("Descripción detallada")]    // El LLM usa esto para decidir cuándo invocarla
public async Task<string> MiFuncion(
    [Description("Descripción del parámetro")] string parametro) { }
```

**Function Calling:**
- El LLM analiza la pregunta del usuario → identifica qué función necesita → la invoca → usa el resultado para responder
- `FunctionChoiceBehavior.Auto()` → el LLM decide automáticamente qué funciones llamar
- `FunctionChoiceBehavior.Required()` → el LLM DEBE invocar al menos una función

**Registro de plugins:**
```csharp
// Opción 1: ImportPluginFromObject (instancia con dependencias inyectadas)
kernel.ImportPluginFromObject(new ClimaPlugin(), "Clima");

// Opción 2: AddFromType (SK crea la instancia — sin parámetros de constructor)
kernelBuilder.Plugins.AddFromType<MathPlugin>("Matematicas");
```

**Plugins pre-construidos:** `TimePlugin`, `ConversationSummaryPlugin`, `HttpPlugin`

---

#### Bloque Práctico (2h)

**Ejercicio 5.1 — ClimaPlugin + MathPlugin (CursoSK.Api)**

```csharp
// Plugins/ClimaPlugin.cs
public class ClimaPlugin
{
    private static readonly Dictionary<string, (int Temp, string Condicion)> _ciudades = new()
    {
        ["Tegucigalpa"] = (28, "Parcialmente nublado"),
        ["San Pedro Sula"] = (33, "Soleado"),
        ["La Ceiba"] = (31, "Lluvioso"),
    };

    [KernelFunction("obtener_clima")]
    [Description("Obtiene el clima actual de una ciudad de Honduras")]
    public string ObtenerClima([Description("Nombre de la ciudad")] string ciudad)
    {
        if (_ciudades.TryGetValue(ciudad, out var data))
            return $"{ciudad}: {data.Temp}°C, {data.Condicion}";
        return $"No tengo datos del clima para {ciudad}";
    }
}

// Plugins/MathPlugin.cs
public class MathPlugin
{
    [KernelFunction("sumar")]
    [Description("Suma dos números")]
    public double Sumar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a + b;

    [KernelFunction("multiplicar")]
    [Description("Multiplica dos números")]
    public double Multiplicar([Description("Primer número")] double a, [Description("Segundo número")] double b) => a * b;
}
```

**Ejercicio 5.2 — AgentController con Function Calling (CursoSK.Api)**

```csharp
// Controllers/AgentController.cs
[HttpPost("consultar")]
public async Task<IActionResult> ConsultarAgente([FromBody] AgentRequest request)
{
    var chatService = _kernel.GetRequiredService<IChatCompletionService>();
    var history = new ChatHistory("Eres un asistente con acceso a plugins de clima y matemáticas.");
    history.AddUserMessage(request.Mensaje);

    var settings = new OpenAIPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    var response = await chatService.GetChatMessageContentAsync(history, settings, _kernel);
    return Ok(new { respuesta = response.Content });
}
```

> **Demostrar Function Calling:**
> - `"¿Cuál es el clima en Tegucigalpa?"` → invoca `obtener_clima("Tegucigalpa")`
> - `"¿Cuánto es 15 × 7 + 3?"` → invoca `multiplicar(15, 7)` → `sumar(105, 3)`

**Ejercicio 5.3 — OnboardingPlugin + OnboardingController (CursoSK.BankingBot)**

```csharp
// Plugins/OnboardingPlugin.cs
public class OnboardingPlugin
{
    [KernelFunction("verificar_identidad")]
    [Description("Verifica la identidad de un cliente por número de identidad")]
    public string VerificarIdentidad([Description("Número de identidad del cliente")] string identidad) { ... }

    [KernelFunction("crear_cuenta")]
    [Description("Crea una cuenta bancaria para un cliente verificado")]
    public string CrearCuenta([Description("Tipo de cuenta")] string tipo, ...) { ... }
}
```

**🔗 Conexión con el Proyecto Real:**
Se estudian los 6 plugins del agente del laboratorio, especialmente `CotizacionPlugin.buscar_cliente_identidad()` y `CitaPlugin.agendar_cita()`.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Plugins/ClimaPlugin.cs` | Plugin de clima con datos simulados |
| CursoSK.Api | `Plugins/MathPlugin.cs` | Plugin de operaciones matemáticas |
| CursoSK.Api | `Controllers/AgentController.cs` | Endpoint con Function Calling automático |
| CursoSK.BankingBot | `Plugins/OnboardingPlugin.cs` | Verificación de identidad y apertura de cuentas |
| CursoSK.BankingBot | `Plugins/CalculadoraFinancieraPlugin.cs` | Cálculos financieros |
| CursoSK.BankingBot | `Controllers/OnboardingController.cs` | Onboarding bancario |

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

```csharp
// Filters/LoggingFilter.cs
public class LoggingFilter : IFunctionInvocationFilter
{
    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        Console.WriteLine($"→ Invocando: {context.Function.PluginName}.{context.Function.Name}");
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();
        Console.WriteLine($"← Completado en {sw.ElapsedMilliseconds}ms: {context.Result}");
    }
}
```

**Plugins OpenAPI:**
- `ImportPluginFromOpenApiAsync()` genera automáticamente un plugin desde una especificación OpenAPI/Swagger
- Permite conectar cualquier API REST como plugin del Kernel

---

#### Bloque Práctico (2h)

**Ejercicio 6.1 — LoggingFilter (CursoSK.Api)**

```csharp
// Registrar en Program.cs:
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, LoggingFilter>();
```

**Ejercicio 6.2 — LegalPlugin + LegalController (CursoSK.BankingBot)**

```csharp
// Plugins/LegalPlugin.cs — Plugin con dependencias inyectadas
public class LegalPlugin
{
    private readonly HttpClient _http;
    public LegalPlugin(HttpClient http) => _http = http;

    [KernelFunction("consultar_normativa")]
    [Description("Consulta la normativa bancaria vigente por tema")]
    public string ConsultarNormativa([Description("Tema legal a consultar")] string tema) { ... }
}

// Registro con ImportPluginFromObject (porque tiene dependencias):
kernel.ImportPluginFromObject(new LegalPlugin(httpClient), "Legal");
```

**Ejercicio 6.3 — PrestamosController (CursoSK.BankingBot)**

```csharp
// Controllers/PrestamosController.cs
[HttpPost("simular")]
public async Task<IActionResult> SimularPrestamo([FromBody] SimulacionRequest request)
{
    // Usar CalculadoraFinancieraPlugin para calcular cuotas
    // El LLM decide qué funciones invocar para armar la simulación
}
```

**Ejercicio 6.4 — Plugin OpenAPI (demostración)**

```csharp
// Importar plugin desde una especificación OpenAPI externa
await kernel.ImportPluginFromOpenApiAsync("WeatherAPI",
    new Uri("https://api.weather.gov/openapi.json"));
```

**🔗 Conexión con el Proyecto Real:**
Se estudia cómo los plugins del laboratorio interactúan con APIs externas (WhatsApp Cloud API, Blob Storage) y cómo se podría agregar auditoría con filtros.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Filters/LoggingFilter.cs` | Interceptor de invocaciones |
| CursoSK.BankingBot | `Plugins/LegalPlugin.cs` | Consulta de normativa bancaria |
| CursoSK.BankingBot | `Filters/AuditFilter.cs` | Auditoría de operaciones |
| CursoSK.BankingBot | `Controllers/PrestamosController.cs` | Simulación de préstamos |
| CursoSK.BankingBot | `Controllers/LegalController.cs` | Consultas legales |

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
| Liquid | `{{variable}}` | Similar a Handlebars |
| YAML | Archivo `.yaml` con config + template | Separar config de template |

**Archivos YAML:**
```yaml
# Prompts/ClasificarIntencion.yaml
name: ClasificarIntencion
template_format: handlebars
template: |
  <message role="system">Clasifica la intención del usuario...</message>
  <message role="user">{{input}}</message>
execution_settings:
  default:
    max_tokens: 100
    temperature: 0.1
input_variables:
  - name: input
    description: Texto del usuario a clasificar
    is_required: true
```

---

#### Bloque Azure Setup (15 min)

**🔧 Crear deployment de embeddings:**

```powershell
cd Scripts/Azure
.\07-crear-deployment-embedding.ps1
```

**Portal:** En el recurso Azure OpenAI → Deployments → Deploy `text-embedding-3-small` (o `text-embedding-ada-002`)

---

#### Bloque Práctico (2h)

**Ejercicio 7.1 — PromptingController (CursoSK.Api)**

```csharp
// Controllers/PromptingController.cs
[HttpPost("zero-shot")]
public async Task<IActionResult> ZeroShot([FromBody] PromptRequest request)
{
    var prompt = $"Clasifica el sentimiento del siguiente texto como Positivo, Negativo o Neutro.\nTexto: {request.Prompt}\nSentimiento:";
    var result = await _kernel.InvokePromptAsync(prompt);
    return Ok(new { tecnica = "Zero-Shot", resultado = result.ToString() });
}

[HttpPost("few-shot")]
public async Task<IActionResult> FewShot([FromBody] PromptRequest request)
{
    var prompt = $"""
        Clasifica el sentimiento:
        Ejemplo: "Me encanta este producto" → Positivo
        Ejemplo: "Es terrible" → Negativo
        Ejemplo: "No está mal" → Neutro
        
        Texto: {request.Prompt}
        Sentimiento:
        """;
    var result = await _kernel.InvokePromptAsync(prompt);
    return Ok(new { tecnica = "Few-Shot", resultado = result.ToString() });
}

[HttpPost("chain-of-thought")]
public async Task<IActionResult> ChainOfThought([FromBody] PromptRequest request)
{
    var prompt = $"""
        Analiza el siguiente texto paso a paso:
        1. Identifica las palabras clave
        2. Determina la emoción predominante
        3. Clasifica como Positivo, Negativo o Neutro
        
        Texto: {request.Prompt}
        
        Análisis:
        """;
    var result = await _kernel.InvokePromptAsync(prompt);
    return Ok(new { tecnica = "Chain-of-Thought", resultado = result.ToString() });
}

[HttpPost("yaml")]
public async Task<IActionResult> DesdeYaml([FromBody] PromptRequest request)
{
    var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "ClasificarIntencion.yaml");
    var yaml = await System.IO.File.ReadAllTextAsync(yamlPath);
    var config = KernelFunctionYaml.ToPromptTemplateConfig(yaml);
    var factory = new HandlebarsPromptTemplateFactory();
    var function = KernelFunctionFactory.CreateFromPrompt(config, factory);
    var result = await _kernel.InvokeAsync(function, new() { ["input"] = request.Prompt });
    return Ok(new { tecnica = "YAML Template", resultado = result.ToString() });
}
```

**Ejercicio 7.2 — Templates YAML para BankingBot**

- Crear `Prompts/ClasificarIntencion.yaml` para clasificar intención del cliente (consulta de saldo, préstamo, transferencia, queja)
- Crear `Prompts/AnalisisDocumento.yaml` para analizar documentos bancarios

**Probar desde Swagger:**
- Comparar respuestas de Zero-Shot vs Few-Shot vs Chain-of-Thought con el mismo texto
- `POST /api/prompting/yaml` → clasificación con template YAML

**🔗 Conexión con el Proyecto Real:**
Se estudia el system prompt del agente del laboratorio con flujo obligatorio, prohibiciones y clasificación de intenciones.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Controllers/PromptingController.cs` | 4 técnicas de prompting |
| CursoSK.Api | `Prompts/ClasificarIntencion.yaml` | Template YAML Handlebars |
| CursoSK.BankingBot | `Prompts/ClasificarIntencion.yaml` | Clasificación bancaria |
| CursoSK.BankingBot | `Prompts/AnalisisDocumento.yaml` | Análisis de documentos |

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
4. **Script**: LLM escribe el guion del podcast con introducciones, transiciones, conclusión
5. **Audio**: TTS sintetiza el audio final

**Fundamentos de Embeddings:**
- **¿Qué es un embedding?** Representación numérica del significado de un texto
- Vector de 1536 dimensiones (ada-002) o 1536 (text-embedding-3-small)
- **Similaridad coseno**: mide qué tan "parecidos" son dos textos
  - `CosineSimilarity = 1.0` → idénticos
  - `CosineSimilarity = 0.0` → sin relación

```
"préstamo hipotecario" → [0.023, -0.041, 0.187, ...]  (1536 dims)
"crédito para vivienda" → [0.019, -0.038, 0.192, ...]  (1536 dims)
CosineSimilarity ≈ 0.95 → ¡Son semánticamente similares!
```

**Diferencia clave:**
```
❌ SIN embeddings:
   Archivo completo (50 páginas) → Se inyecta como contexto → 50,000+ tokens consumidos

✅ CON embeddings + Vector Storage:
   Archivo (50 páginas) → 200 fragmentos → 200 vectores
   Pregunta → Se buscan los 3-5 fragmentos más relevantes → ~500 tokens
```

---

#### Bloque Práctico (2h 15min)

**Ejercicio 8.1 — VectorStoreService + DocumentoVectorial (CursoSK.Api/BankingBot)**

```csharp
// Models/DocumentoVectorial.cs
public class DocumentoVectorial
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Texto { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
    public ReadOnlyMemory<float> Embedding { get; set; }
}
```

```csharp
// Services/VectorStoreService.cs
public class VectorStoreService
{
    private readonly ConcurrentDictionary<string, DocumentoVectorial> _documentos = new();
    private readonly ITextEmbeddingGenerationService _embeddingService;

    public async Task IndexarDocumento(string texto, string fuente)
    {
        var embedding = await _embeddingService.GenerateEmbeddingAsync(texto);
        var doc = new DocumentoVectorial { Texto = texto, Fuente = fuente, Embedding = embedding };
        _documentos.TryAdd(doc.Id, doc);
    }

    public async Task<List<(DocumentoVectorial Doc, double Score)>> BuscarSimilares(string query, int top = 3)
    {
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(query);
        return _documentos.Values
            .Select(d => (Doc: d, Score: (double)TensorPrimitives.CosineSimilarity(
                queryEmbedding.Span, d.Embedding.Span)))
            .OrderByDescending(x => x.Score)
            .Take(top)
            .ToList();
    }
}
```

**Ejercicio 8.2 — Registrar servicio de embeddings en Program.cs**

```csharp
// Program.cs — Agregar servicio de embeddings
kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: builder.Configuration["LLMSettings:AzureOpenAI:EmbeddingDeployment"]!,
    endpoint: builder.Configuration["LLMSettings:AzureOpenAI:Endpoint"]!,
    apiKey: builder.Configuration["LLMSettings:AzureOpenAI:ApiKey"]!);
```

**BankingBot:** Crear `VectorStoreService` y `DocumentoVectorial` para indexar normativas bancarias.

**Probar:** Indexar algunos textos de prueba y buscar por similaridad semántica.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Services/VectorStoreService.cs` | Vector store in-memory con CosineSimilarity |
| CursoSK.Api | `Models/DocumentoVectorial.cs` | Modelo de documento vectorial |
| CursoSK.Api | `Program.cs` | + servicio de embeddings |
| CursoSK.BankingBot | `Services/VectorStoreService.cs` | Vector store bancario |
| CursoSK.BankingBot | `Models/DocumentoVectorial.cs` | Modelo vectorial |

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

**Patrón RAG completo:**

```
┌─────────────────────────────────────────────────────────────┐
│               FLUJO RAG (Retrieval-Augmented Generation)    │
├─────────────────────────────────────────────────────────────┤
│  1. INDEXACIÓN (una vez):                                   │
│     Documentos (.pdf/.txt) → Chunking → Embedding → Store  │
│                                                             │
│  2. CONSULTA (cada request):                                │
│     Pregunta → Embedding → Búsqueda vectorial → Top K      │
│     → Inyectar fragmentos como contexto → LLM responde     │
└─────────────────────────────────────────────────────────────┘
```

**Cuándo RAG vs. Fine-tuning vs. Prompt Engineering:**

| Enfoque | Cuándo usar | Costo |
|---|---|---|
| Prompt Engineering | Datos pequeños, pocos documentos | Bajo |
| RAG | Datos que cambian frecuentemente, documentos grandes | Medio |
| Fine-tuning | Comportamiento especializado, lenguaje/estilo específico | Alto |

**RAG en Foundry (portal) vs. RAG en código:**

| Aspecto | Foundry Web | Código (SK + API) |
|---|---|---|
| Chunking | Automático | Manual — se controla el tamaño |
| Embedding | Automático | Explícito con `ITextEmbeddingGenerationService` |
| Vector Store | Azure AI Search (automático) | In-Memory, Azure AI Search, o cualquier conector |
| Control | Bajo ("caja negra") | Total |
| Mejor para | Prototipos, demos rápidas | Producción, APIs, integración |

**SK Agent Framework — ChatCompletionAgent:**
```csharp
var agent = new ChatCompletionAgent
{
    Name = "AgenteRAG",
    Instructions = "Responde preguntas usando el contexto proporcionado...",
    Kernel = kernel,
    Arguments = new KernelArguments(settings)
};
```

---

#### Bloque Azure Setup (15 min)

**🔧 Azure AI Search (opcional):**

```powershell
# Opción CLI:
cd Scripts/Azure
.\09-crear-ai-search.ps1

# Portal: Create a resource → "Azure AI Search" → Free tier → mismo RG
```

**🔧 Carga de archivos en Foundry (demostración):**
1. Ir a https://ai.azure.com → abrir proyecto → seleccionar deployment
2. En el Playground → **"Adjuntar archivos"** (Add your data)
3. Arrastrar documentos (PDF, TXT, DOCX, MD)
4. Foundry procesa automáticamente: extrae texto → chunks → embeddings → índice vectorial
5. El modelo puede consultar el índice para responder preguntas

**🛡️ Guardrails (Content Safety):**
- En el deployment → "Asignar límite de protección"
- **Microsoft.DefaultV2**: Filtros de odio, autolesiones, sexual, violencia (nivel medio)
- Protege tanto entrada como salida

---

#### Bloque Práctico (1h 45min)

**Ejercicio 9.1 — RAGController (CursoSK.Api)**

```csharp
// Controllers/RAGController.cs
[HttpPost("indexar")]
public async Task<IActionResult> IndexarDocumento([FromBody] IndexarRequest request)
{
    await _vectorStore.IndexarDocumento(request.Texto, request.Fuente);
    return Ok(new { mensaje = "Documento indexado exitosamente" });
}

[HttpPost("buscar")]
public async Task<IActionResult> BuscarSimilares([FromBody] BuscarRequest request)
{
    var resultados = await _vectorStore.BuscarSimilares(request.Query, request.Top);
    return Ok(resultados.Select(r => new { texto = r.Doc.Texto, fuente = r.Doc.Fuente, score = r.Score }));
}

[HttpPost("consultar")]
public async Task<IActionResult> ConsultarRAG([FromBody] RAGConsultaRequest request)
{
    // 1. Buscar fragmentos relevantes
    var fragmentos = await _vectorStore.BuscarSimilares(request.Pregunta, 3);
    
    // 2. Construir contexto
    var contexto = string.Join("\n---\n", fragmentos.Select(f => f.Doc.Texto));
    
    // 3. Prompt con contexto inyectado
    var prompt = $"""
        Responde ÚNICAMENTE basándote en el siguiente contexto.
        Si la información no está en el contexto, di "No tengo información suficiente."
        
        CONTEXTO:
        {contexto}
        
        PREGUNTA: {request.Pregunta}
        """;
    
    var result = await _kernel.InvokePromptAsync(prompt);
    return Ok(new { respuesta = result.ToString(), fragmentosUsados = fragmentos.Count });
}

[HttpPost("seed")]
public async Task<IActionResult> SeedFAQ()
{
    var faqs = new[] {
        ("¿Qué es Semantic Kernel?", "Semantic Kernel es un SDK open-source de Microsoft..."),
        ("¿Qué es RAG?", "Retrieval-Augmented Generation es un patrón que..."),
        // ... más FAQs
    };
    foreach (var (pregunta, respuesta) in faqs)
        await _vectorStore.IndexarDocumento($"{pregunta}\n{respuesta}", "FAQ");
    return Ok(new { mensaje = $"{faqs.Length} FAQs indexadas" });
}
```

**Ejercicio 9.2 — RAG para leyes bancarias (CursoSK.BankingBot)**

```csharp
// Controllers/RAGController.cs (BankingBot)
[HttpPost("indexar/leyes")]
public async Task<IActionResult> IndexarLeyes()
{
    // Leer archivos de Docs/Leyes/*.txt
    // Dividir en chunks de ~500 tokens
    // Indexar cada chunk con el VectorStoreService
}
```

- Indexar leyes hondureñas: Decreto 129-2004, 144-2014, 170-2016, Resolución CNBS-GES-041-2019
- Consultar: "¿Cuáles son los requisitos para abrir una cuenta bancaria?"

**Probar desde Swagger:**
1. `POST /api/rag/seed` → cargar FAQs de ejemplo
2. `POST /api/rag/buscar` → `{ "query": "¿Qué es Semantic Kernel?", "top": 3 }`
3. `POST /api/rag/consultar` → `{ "pregunta": "¿Qué es RAG y para qué sirve?" }`

**🔗 Conexión con el Proyecto Real:**
Se contrasta el enfoque actual del agente del laboratorio (inyectar `InfoLaboratorio.txt` completo como contexto) con el enfoque RAG que reduce drásticamente los tokens consumidos.

---

**Archivos creados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `Controllers/RAGController.cs` | Endpoints de indexación, búsqueda y consulta RAG |
| CursoSK.BankingBot | `Controllers/RAGController.cs` | RAG para leyes bancarias |
| CursoSK.BankingBot | `Services/LegalIndexingService.cs` | Indexación de documentos legales |
| CursoSK.BankingBot | `Docs/Leyes/*.txt` | Documentos legales hondureños |

---

---

### 📅 SESIÓN 10 — Deployment Azure + Foundry + Integración Final (3h)

| Campo | Detalle |
|-------|---------|
| **Tema Principal** | Deploy a Azure App Service + Microsoft Foundry completo + demo final |
| **Subtemas** | Azure App Service, `dotnet publish`, `az webapp deploy`, Foundry guardrails, Foundry Agent Service, producción |
| **Herramientas** | Azure App Service, Azure CLI, Microsoft Foundry, Swagger |
| **Rama Git** | `sesion/10` (merge a `main`) |
| **Scripts Azure** | `10-deploy-app-service.ps1`, `10-foundry-setup.ps1` |
| **Duración** | 3 horas |

---

#### Bloque Teórico (45 min)

**Azure App Service:**
- Plataforma PaaS para hospedar aplicaciones web .NET
- Plan de App Service: Free (F1), Basic (B1), Standard (S1)
- Variables de entorno para producción (no usar `appsettings.json` para secretos)

**Microsoft Foundry — Características Avanzadas:**

| Característica | Descripción |
|---|---|
| **Playground** | Probar modelos con chat interactivo |
| **Adjuntar archivos** | Crear índices vectoriales automáticamente (RAG zero-code) |
| **Guardrails** | Content Safety: filtros de odio, autolesiones, sexual, violencia |
| **Agent Service** | 3 tipos: Solicitud (sin código), Flujo de trabajo (YAML), Hospedados (contenedor) |

**Tipos de Agente en Foundry Agent Service:**

| Tipo | Código | Orquestación | Mejor Para |
|---|---|---|---|
| **Agentes de Solicitud** | No | Agente único, administrado | Prototipos, tareas simples |
| **Agentes de Flujo de Trabajo** (preview) | YAML opcional | Multi-agente, bifurcación | Automatización multi-paso |
| **Agentes Hospedados** (preview) | Sí (contenedor) | Lógica personalizada | Control total (SK, LangGraph) |

**Ajustes de producción:**
- Variables de entorno en App Service (no en appsettings.json)
- `appsettings.Production.json` para config no sensible
- HTTPS obligatorio
- Logging con Application Insights (opcional)

---

#### Bloque Azure Setup (45 min)

**🔧 Deploy a Azure App Service:**

**Opción A — Script PowerShell:**
```powershell
cd Scripts/Azure
.\10-deploy-app-service.ps1
# El script: selecciona proyecto → dotnet publish → crea App Service → configura variables → zip deploy
```

**Opción B — Portal de Azure:**
1. **Create a resource** → **Web App**
2. Configurar:
   - Resource Group: `rg-cursosk`
   - Name: nombre único (ej: `cursosk-api`)
   - Runtime stack: `.NET 9 (STS)`
   - Region: misma que los recursos OpenAI
   - Plan: `Free F1` (para demo) o `Basic B1`
3. **Review + Create** → **Create**
4. Ir al recurso → **Configuration** → **Application settings**:
   - `LLMSettings__Provider` = `AzureOpenAI`
   - `LLMSettings__AzureOpenAI__Endpoint` = `https://TU-RECURSO.openai.azure.com/`
   - `LLMSettings__AzureOpenAI__ApiKey` = `TU-API-KEY`
   - `LLMSettings__AzureOpenAI__ChatDeployment` = `gpt-35-turbo-16k`
5. **Deployment Center** → configurar desde GitHub o zip deploy

**Opción C — Azure CLI manual:**
```bash
# Publicar el proyecto
cd CursoSK.Api
dotnet publish -c Release -o ./publish

# Crear zip
cd publish
Compress-Archive -Path * -DestinationPath ../deploy.zip

# Crear App Service Plan + Web App
az appservice plan create --name cursosk-plan --resource-group rg-cursosk --sku F1
az webapp create --name cursosk-api --resource-group rg-cursosk --plan cursosk-plan --runtime "DOTNETCORE:9.0"

# Configurar variables de entorno
az webapp config appsettings set --name cursosk-api --resource-group rg-cursosk --settings \
    LLMSettings__Provider=AzureOpenAI \
    LLMSettings__AzureOpenAI__Endpoint=https://TU-RECURSO.openai.azure.com/ \
    LLMSettings__AzureOpenAI__ApiKey=TU-API-KEY

# Deploy
az webapp deploy --name cursosk-api --resource-group rg-cursosk --src-path ../deploy.zip --type zip
```

**🔧 Configurar Foundry (demostración):**
- Ver `Scripts/Azure/10-foundry-setup.ps1` para guía completa paso a paso
- 5 partes: Acceso, Playground, File Upload/RAG, Guardrails, Agent Service

---

#### Bloque Práctico (1h 30min)

**Ejercicio 10.1 — Deploy CursoSK.Api a Azure App Service**

1. Ejecutar script de deploy o seguir pasos del portal
2. Verificar que Swagger funciona en la URL pública
3. Probar endpoints desde Swagger en producción

**Ejercicio 10.2 — Foundry: Subir documentos y crear índice vectorial**

1. Ir a https://ai.azure.com → Playground
2. Adjuntar documentos legales
3. Probar queries en el Playground
4. Comparar resultados: RAG en código (BankingBot) vs. RAG en Foundry

**Ejercicio 10.3 — Demo completa del BankingBot**

Probar todos los endpoints del BankingBot en secuencia:
1. `POST /api/onboarding/iniciar` → Onboarding con Function Calling
2. `POST /api/chat/mensaje` → Chat con contexto bancario
3. `POST /api/legal/consultar/rag` → Consulta legal con RAG
4. `POST /api/audio/transcribir` → Transcripción de audio (si key es válida)
5. `POST /api/rag/indexar/leyes` + `POST /api/rag/buscar` → RAG vectorial

**Ejercicio 10.4 — Merge a main y tag de release**

```bash
git checkout main
git merge sesion/10
git tag v1.0.0 -m "Curso completo - 10 sesiones"
git push origin main --tags
```

**🔗 Conexión con el Proyecto Real:**
Revisión del deploy en producción del agente del laboratorio en Azure App Service: `dotnet clean` + `dotnet publish` + recrear `.zip` + `az webapp deploy`.

---

**Archivos creados/modificados esta sesión:**

| Proyecto | Archivo | Descripción |
|---|---|---|
| CursoSK.Api | `appsettings.Production.json` | Configuración de producción |
| Scripts/Azure | `10-deploy-app-service.ps1` | Deploy a App Service |
| Scripts/Azure | `10-foundry-setup.ps1` | Guía de Foundry |

---

---

## Resumen: Archivos por Sesión

| Sesión | CursoSK.Api | CursoSK.BankingBot | Scripts/Azure |
|---|---|---|---|
| **1** | Program.cs, KernelController, appsettings.json, DTOs | Program.cs, appsettings.json | 01-crear-recurso-openai.ps1 |
| **2** | MultimodalController, Program.cs (+multimodal) | AudioController | 02-crear-deployment-whisper.ps1 |
| **3** | BlogService, BlogController, ChatSessionService, ChatController | ConversationService, ChatController | — |
| **4** | ChatModels, AppDbContext, ChatController (+imagen), DTOs | Models bancarios, BankingDbContext | — |
| **5** | ClimaPlugin, MathPlugin, AgentController | OnboardingPlugin, CalculadoraFinancieraPlugin, OnboardingController | — |
| **6** | LoggingFilter | LegalPlugin, AuditFilter, PrestamosController, LegalController | — |
| **7** | PromptingController, ClasificarIntencion.yaml | ClasificarIntencion.yaml, AnalisisDocumento.yaml | 07-crear-deployment-embedding.ps1 |
| **8** | VectorStoreService, DocumentoVectorial, Program.cs (+embeddings) | VectorStoreService, DocumentoVectorial | — |
| **9** | RAGController | RAGController, LegalIndexingService, Docs/Leyes/*.txt | 09-crear-ai-search.ps1 |
| **10** | appsettings.Production.json | — | 10-deploy-app-service.ps1, 10-foundry-setup.ps1 |

---

## Evaluación y Verificación

| Criterio | Herramienta |
|---|---|
| Ambos proyectos compilan | `dotnet build CursoSK.Api` + `dotnet build CursoSK.BankingBot` |
| Cada rama sesion/XX compila | `git checkout sesion/XX && dotnet build` |
| Endpoints funcionan en Swagger | Probar manualmente cada endpoint |
| RAG devuelve resultados relevantes | Indexar docs → buscar → verificar scores > 0.7 |
| Deploy funciona en Azure | URL pública + Swagger accesible |
| Scripts PowerShell son válidos | `pwsh -c "& { . .\script.ps1 }"` |

---

## Recursos Adicionales

- [Documentación Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/)
- [Microsoft Foundry](https://learn.microsoft.com/es-mx/azure/foundry/)
- [Azure OpenAI Service](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
- [Azure CLI Reference](https://learn.microsoft.com/cli/azure/)
- [Repositorio del Curso](https://github.com/desarrollossoftware607/CursoSK-BankingBot)
