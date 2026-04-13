# Paso a Paso — Sesión 2: Servicios Multimodales

> **Rama Git:** `sesion/02`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Controllers/MultimodalController.cs` | **Crear** |
| `Program.cs` | Modificar (agregar servicios multimodales) |

---

## 1. Controllers/MultimodalController.cs

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

---

## 2. Modificar Program.cs — Agregar servicios multimodales

Agregar al `kernelBuilder` (DESPUÉS de `AddAzureOpenAIChatCompletion`):

```csharp
// Servicios multimodales (Sesión 2)
#pragma warning disable SKEXP0010
kernelBuilder.AddOpenAITextToImage(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "dall-e-3");
kernelBuilder.AddOpenAITextToAudio(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "tts-1");
kernelBuilder.AddOpenAIAudioToText(
    apiKey: builder.Configuration["LLMSettings:OpenAI:ApiKey"]!, modelId: "whisper-1");
#pragma warning restore SKEXP0010
```

---

## 3. Probar

```powershell
dotnet run
```

- `POST /api/multimodal/stream` → `{ "prompt": "Explica qué es machine learning" }` → streaming token por token (SSE)

---

## 4. Azure Setup

Ejecutar el script para crear el deployment de Whisper:

```powershell
cd Scripts/Azure
.\02-crear-deployment-whisper.ps1
```
