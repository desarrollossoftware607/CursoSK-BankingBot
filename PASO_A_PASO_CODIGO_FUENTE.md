# Paso a Paso — Sesión 10: Deployment Azure + Foundry + Integración Final

> **Rama Git:** `sesion/10` (merge a main)

---

## 1. Deploy a Azure App Service

```powershell
cd Scripts/Azure
.\10-deploy-app-service.ps1
```

El script ejecuta:
1. `dotnet clean` + `dotnet publish -c Release`
2. Crea zip del output
3. Crea App Service Plan + Web App
4. Configura variables de entorno (LLMSettings)
5. Ejecuta `az webapp deploy --type zip`

---

## 2. Configurar Microsoft Foundry

Seguir la guía en: `Scripts/Azure/10-foundry-setup.ps1`

---

## 3. Probar en Azure

Una vez desplegado, abrir Swagger en la URL pública de tu App Service:

```
https://tu-app.azurewebsites.net/swagger
```

Probar todos los endpoints del curso:
- `POST /api/kernel/prompt`
- `POST /api/multimodal/stream`
- `POST /api/blog/generar`
- `POST /api/chat/{id}/mensaje`
- `POST /api/agent/consultar`
- `POST /api/prompting/yaml`
- `POST /api/rag/consultar`

---

## 4. Merge a main

```bash
git checkout main
git merge sesion/10
git tag v1.0.0 -m "Curso completo - 10 sesiones"
git push origin main --tags
```

---

## Program.cs — Versión Final Completa

```csharp
using Microsoft.SemanticKernel;
using Microsoft.EntityFrameworkCore;
using CursoSK.Api.Plugins;
using CursoSK.Api.Filters;
using CursoSK.Api.Services;
using CursoSK.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CursoSK.Api", Version = "v1" });
});

// Semantic Kernel
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

// Servicios multimodales (Sesión 2)
#pragma warning disable SKEXP0010
kernelBuilder.AddOpenAITextToImage(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "dall-e-3");
kernelBuilder.AddOpenAITextToAudio(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "tts-1");
kernelBuilder.AddOpenAIAudioToText(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "whisper-1");

// Embeddings (Sesión 8)
kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
    deploymentName: builder.Configuration["LLMSettings:Embedding:DeploymentName"]!,
    endpoint: builder.Configuration["LLMSettings:Embedding:Endpoint"]!,
    apiKey: builder.Configuration["LLMSettings:Embedding:ApiKey"]!);
#pragma warning restore SKEXP0010

// Plugins (Sesión 5)
kernelBuilder.Plugins.AddFromObject(new ClimaPlugin(), "Clima");
kernelBuilder.Plugins.AddFromType<MathPlugin>("Matematica");

// Filtros (Sesión 6)
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, LoggingFilter>();

builder.Services.AddSingleton(kernelBuilder.Build());

// Services
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<ChatSessionService>();
builder.Services.AddSingleton<VectorStoreService>();

// EF Core (Sesión 4)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=cursosk.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
```

---

## DTOs/Requests.cs — Versión Final Completa

```csharp
namespace CursoSK.Api.DTOs;

// Sesión 1
public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);

// Sesión 3
public record BlogRequest(string Tema);
public record ChatMensajeRequest(string Mensaje);

// Sesión 4
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);

// Sesión 9
public record IndexarDocumentoRequest(string Titulo, string Contenido, string Categoria);
public record BusquedaSemanticaRequest(string Consulta, int Top = 3);
```
