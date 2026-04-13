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

    /// <summary>
    /// Envía un prompt al agente con Function Calling automático.
    /// El LLM decide qué plugins invocar según la consulta.
    /// </summary>
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

    /// <summary>Lista todos los plugins y funciones registrados en el Kernel.</summary>
    [HttpGet("plugins")]
    public IActionResult ListarPlugins()
    {
        var plugins = _kernel.Plugins.Select(p => new
        {
            plugin = p.Name,
            funciones = p.Select(f => new
            {
                nombre = f.Name,
                descripcion = f.Description,
                parametros = f.Metadata.Parameters.Select(pr => new
                {
                    nombre = pr.Name,
                    descripcion = pr.Description,
                    tipo = pr.ParameterType?.Name,
                    requerido = pr.IsRequired
                })
            })
        });
        return Ok(plugins);
    }
}
