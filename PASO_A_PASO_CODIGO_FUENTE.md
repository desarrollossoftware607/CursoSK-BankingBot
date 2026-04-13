# Paso a Paso — Sesión 5: Plugins Nativos y Function Calling

> **Rama Git:** `sesion/05`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Plugins/ClimaPlugin.cs` | **Crear** |
| `Plugins/MathPlugin.cs` | **Crear** |
| `Controllers/AgentController.cs` | **Crear** |
| `Program.cs` | Modificar (registrar plugins) |

---

## 1. Plugins/ClimaPlugin.cs

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

---

## 2. Plugins/MathPlugin.cs

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

---

## 3. Controllers/AgentController.cs

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

---

## 4. Registrar plugins en Program.cs

Agregar ANTES de `kernelBuilder.Build()`:

```csharp
// Plugins (Sesión 5)
kernelBuilder.Plugins.AddFromObject(new ClimaPlugin(), "Clima");
kernelBuilder.Plugins.AddFromType<MathPlugin>("Matematica");
```

No olvidar el `using` al inicio:

```csharp
using CursoSK.Api.Plugins;
```

---

## 5. Probar Function Calling

```powershell
dotnet run
```

- `POST /api/agent/consultar` → `{ "prompt": "¿Cuál es el clima en Tegucigalpa?" }` → El LLM invoca `ClimaPlugin.obtener_clima`
- `POST /api/agent/consultar` → `{ "prompt": "¿Cuánto es 15 × 7 + 3?" }` → El LLM invoca `MathPlugin.multiplicar` y `MathPlugin.sumar`
- `GET /api/agent/plugins` → Lista todas las funciones disponibles
