using System.Text.Json;
using Microsoft.SemanticKernel;

namespace CursoSK.BankingBot.Filters;

/// <summary>Filtro de auditoría que registra todas las invocaciones de plugins.</summary>
public class AuditFilter : IFunctionInvocationFilter
{
    private readonly ILogger<AuditFilter> _logger;

    public AuditFilter(ILogger<AuditFilter> logger) => _logger = logger;

    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        _logger.LogInformation("🔧 Invocando: {Plugin}.{Function} | Args: {Args}",
            context.Function.PluginName,
            context.Function.Name,
            JsonSerializer.Serialize(context.Arguments));

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await next(context);
        sw.Stop();

        var resultado = context.Result?.ToString();
        _logger.LogInformation("✅ {Plugin}.{Function} completado en {Duration}ms",
            context.Function.PluginName,
            context.Function.Name,
            sw.ElapsedMilliseconds);
    }
}
