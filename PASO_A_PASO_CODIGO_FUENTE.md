# Paso a Paso — Sesión 6: Plugins Avanzados + Filtros + OpenAPI

> **Rama Git:** `sesion/06`

---

## Archivos a crear/modificar esta sesión

| Archivo | Acción |
|---|---|
| `Filters/LoggingFilter.cs` | **Crear** |
| `Program.cs` | Modificar (registrar filtro) |

---

## 1. Filters/LoggingFilter.cs

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

---

## 2. Registrar filtro en Program.cs

Agregar al `kernelBuilder`:

```csharp
// Filtros (Sesión 6)
kernelBuilder.Services.AddSingleton<IFunctionInvocationFilter, LoggingFilter>();
```

No olvidar el `using`:

```csharp
using CursoSK.Api.Filters;
```

---

## 3. Probar

```powershell
dotnet run
```

- `POST /api/agent/consultar` → `{ "prompt": "¿Cuál es el clima en San Pedro Sula?" }`
- Observar los logs en la consola: se registra cada invocación de función con el tiempo de ejecución
