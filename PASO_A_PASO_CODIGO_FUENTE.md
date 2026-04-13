# Paso a Paso — Sesión 7: Prompting Avanzado + Templates YAML

> **Rama Git:** `sesion/07`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Controllers/PromptingController.cs` | **Crear** |
| `Prompts/ClasificarIntencion.yaml` | **Crear** |
| `CursoSK.Api.csproj` | Modificar (copiar YAML al output) |

---

## Instalar paquetes NuGet adicionales

```powershell
dotnet add package Microsoft.SemanticKernel.PromptTemplates.Handlebars --version 1.48.0
dotnet add package Microsoft.SemanticKernel.Yaml --version 1.48.0
```

---

## 1. Controllers/PromptingController.cs

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

---

## 2. Prompts/ClasificarIntencion.yaml

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

---

## 3. Modificar CursoSK.Api.csproj

Agregar para que los archivos YAML se copien al output:

```xml
<ItemGroup>
  <None Update="Prompts\**\*.yaml">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

---

## 4. Probar

```powershell
dotnet run
```

- `POST /api/prompting/zero-shot` → `{ "prompt": "Traduce 'hello world' al español" }`
- `POST /api/prompting/few-shot` → `{ "prompt": "El servicio fue excelente, muy recomendado" }`
- `POST /api/prompting/chain-of-thought` → `{ "prompt": "Si tengo 3 cajas con 12 manzanas cada una y regalo 8, ¿cuántas me quedan?" }`
- `POST /api/prompting/yaml` → `{ "prompt": "Quiero solicitar un préstamo de 50,000 lempiras" }`

---

## 5. Azure Setup

Ejecutar el script para crear el deployment de embeddings (preparación para sesión 8):

```powershell
cd Scripts/Azure
.\07-crear-deployment-embedding.ps1
```
