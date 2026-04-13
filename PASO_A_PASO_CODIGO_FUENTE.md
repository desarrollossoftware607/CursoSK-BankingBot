# Paso a Paso — Sesión 1: Fundamentos SK + Setup Azure

> **Rama Git:** `sesion/01`

---

## 1. Preparar el Entorno de Desarrollo

### 1.1 — Instalar .NET 9 SDK

Descargar e instalar desde: https://dotnet.microsoft.com/download/dotnet/9.0

Verificar la instalación:
```powershell
dotnet --version
# Debe mostrar 9.0.x
```

### 1.2 — Instalar Visual Studio Code

Descargar desde: https://code.visualstudio.com/

Extensiones recomendadas:
- **C# Dev Kit** (Microsoft)
- **REST Client** (Huachao Mao) — para probar endpoints sin Postman

### 1.3 — Instalar Git

Descargar desde: https://git-scm.com/downloads

Verificar:
```powershell
git --version
```

### 1.4 — Clonar el repositorio del curso

```powershell
git clone https://github.com/desarrollossoftware607/CursoSK-BankingBot.git
cd CursoSK-BankingBot
git checkout sesion/01
```

---

## 2. Crear el Proyecto desde Cero

### 2.1 — Crear el proyecto Web API

```powershell
dotnet new webapi -n CursoSK.Api --use-controllers
cd CursoSK.Api
```

### 2.2 — Instalar paquetes NuGet

```powershell
dotnet add package Microsoft.SemanticKernel --version 1.48.0
dotnet add package Swashbuckle.AspNetCore --version 6.9.0
```

### 2.3 — Crear estructura de carpetas

```powershell
"Controllers","Services","Plugins","Filters","Models","Data","DTOs","Prompts" | ForEach-Object { New-Item -ItemType Directory -Name $_ -Force }
```

### 2.4 — Eliminar archivos de plantilla

Eliminar los archivos generados por `dotnet new webapi` que no necesitamos:
- `WeatherForecast.cs`
- `Controllers/WeatherForecastController.cs`

---

## 3. Crear los Archivos del Proyecto

### 3.1 — appsettings.json

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
      "DeploymentName": "gpt-4o-mini",
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

> **IMPORTANTE:** Crear `appsettings.Development.json` con tus API keys reales. Este archivo está en `.gitignore` y NO se sube al repositorio.

### 3.2 — Program.cs

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

### 3.3 — DTOs/Requests.cs

```csharp
namespace CursoSK.Api.DTOs;

public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
```

### 3.4 — Controllers/KernelController.cs

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

---

## 4. Ejecutar y Probar

```powershell
dotnet run
```

Abrir en el navegador: http://localhost:5192/swagger

**Probar endpoints:**
- `POST /api/kernel/prompt` → `{ "prompt": "¿Qué es Semantic Kernel?" }`
- `POST /api/kernel/prompt/configurado` → `{ "prompt": "Explica qué es .NET", "maxTokens": 100, "temperature": 0.5 }`

---

## 5. Azure Setup

Ejecutar el script para crear los recursos de Azure OpenAI:

```powershell
cd Scripts/Azure
. .\00-variables.ps1
.\01-crear-recurso-openai.ps1
```

Este script crea:
- Resource Group
- Azure OpenAI resource
- Deployment de modelo de chat (gpt-4o-mini)
