using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using CursoSK.BankingBot.DTOs;

namespace CursoSK.BankingBot.Controllers;

/// <summary>Asistente de onboarding bancario con Function Calling automático.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("🏦 Onboarding Bancario")]
public class OnboardingController : ControllerBase
{
    private readonly Kernel _kernel;

    public OnboardingController(Kernel kernel) => _kernel = kernel;

    /// <summary>
    /// Consulta al asistente de onboarding bancario con lenguaje natural.
    /// El agente invoca automáticamente plugins según la necesidad:
    /// requisitos, simulación, elegibilidad, estado de solicitud, etc.
    /// </summary>
    [HttpPost("consultar")]
    public async Task<IActionResult> Consultar([FromBody] PromptRequest request)
    {
        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var systemPrompt = """
            Eres un asistente bancario experto en onboarding y préstamos.
            Tu rol es guiar al cliente paso a paso en el proceso de solicitud de préstamos.
            - Usa un tono profesional pero amigable.
            - Siempre indica los requisitos aplicables.
            - Cita las normativas cuando sea relevante.
            - Si el cliente pregunta por montos, simula la cuota automáticamente.
            - Verifica elegibilidad cuando tengas datos de ingreso.
            """;

        var prompt = $"{systemPrompt}\n\nCliente: {request.Prompt}";
        var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings));
        return Ok(new { respuesta = result.ToString() });
    }

    /// <summary>Simula un préstamo: cuota mensual, total a pagar, total intereses.</summary>
    [HttpPost("simular")]
    public IActionResult Simular([FromBody] SimulacionRequest request)
    {
        var tasaMensual = request.TasaAnual / 100 / 12;
        var cuota = request.Monto * tasaMensual * (decimal)Math.Pow((double)(1 + tasaMensual), request.PlazoMeses)
                    / ((decimal)Math.Pow((double)(1 + tasaMensual), request.PlazoMeses) - 1);
        var totalPagar = cuota * request.PlazoMeses;

        return Ok(new
        {
            monto = request.Monto,
            plazoMeses = request.PlazoMeses,
            tasaAnual = request.TasaAnual,
            cuotaMensual = Math.Round(cuota, 2),
            totalPagar = Math.Round(totalPagar, 2),
            totalIntereses = Math.Round(totalPagar - request.Monto, 2)
        });
    }

    /// <summary>Lista los plugins y funciones disponibles del agente.</summary>
    [HttpGet("plugins")]
    public IActionResult ListarPlugins()
    {
        var plugins = _kernel.Plugins.Select(p => new
        {
            nombre = p.Name,
            funciones = p.Select(f => new
            {
                nombre = f.Name,
                descripcion = f.Description,
                parametros = f.Metadata.Parameters.Select(par => new
                {
                    par.Name,
                    par.Description,
                    tipo = par.ParameterType?.Name,
                    par.IsRequired
                })
            })
        });
        return Ok(plugins);
    }
}
