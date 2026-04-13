using Microsoft.SemanticKernel;

namespace CursoSK.Api.Filters;

public class LoggingFilter : IFunctionInvocationFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger) => _logger = logger;

    public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
    {
        var pluginName = context.Function.PluginName;
        var functionName = context.Function.Name;
        var args = string.Join(", ", context.Arguments.Select(a => $"{a.Key}={a.Value}"));

        _logger.LogInformation("▶ Invocando {Plugin}.{Function}({Args})", pluginName, functionName, args);
        var sw = System.Diagnostics.Stopwatch.StartNew();

        await next(context);

        sw.Stop();
        _logger.LogInformation("✔ {Plugin}.{Function} completado en {Ms}ms → {Result}",
            pluginName, functionName, sw.ElapsedMilliseconds,
            context.Result?.ToString()?[..Math.Min(200, context.Result?.ToString()?.Length ?? 0)]);
    }
}
