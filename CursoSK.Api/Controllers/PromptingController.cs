using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using CursoSK.Api.DTOs;
using System.Text.RegularExpressions;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("7️⃣ Prompting — Sesión 7")]
public class PromptingController : ControllerBase
{
    private readonly Kernel _kernel;
    public PromptingController(Kernel kernel) => _kernel = kernel;

    /// <summary>Zero-Shot: el LLM responde sin ejemplos previos.</summary>
    [HttpPost("zero-shot")]
    public async Task<IActionResult> ZeroShot([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { tecnica = "Zero-Shot", respuesta = result.ToString() });
    }

    /// <summary>Few-Shot: se proporcionan ejemplos para guiar la respuesta.</summary>
    [HttpPost("few-shot")]
    public async Task<IActionResult> FewShot([FromBody] PromptRequest request)
    {
        var prompt = $"""
            Clasifica el sentimiento del texto como POSITIVO, NEGATIVO o NEUTRO.

            Ejemplos:
            - "El servicio fue excelente" → POSITIVO
            - "Tuve problemas con el cajero" → NEGATIVO
            - "Realicé una transferencia" → NEUTRO

            Texto: {request.Prompt}
            Sentimiento:
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { tecnica = "Few-Shot", texto = request.Prompt, respuesta = result.ToString() });
    }

    /// <summary>Chain of Thought: razonamiento paso a paso.</summary>
    [HttpPost("chain-of-thought")]
    public async Task<IActionResult> ChainOfThought([FromBody] PromptRequest request)
    {
        var prompt = $"""
            Analiza el siguiente problema paso a paso. Muestra tu razonamiento completo antes de dar la respuesta final.

            Problema: {request.Prompt}

            Razonamiento paso a paso:
            """;
        var result = await _kernel.InvokePromptAsync(prompt);
        return Ok(new { tecnica = "Chain-of-Thought", respuesta = result.ToString() });
    }

    /// <summary>Prompt desde archivo YAML (Handlebars template).</summary>
    [HttpPost("yaml")]
    public async Task<IActionResult> DesdeYaml([FromBody] PromptRequest request)
    {
        var yamlPath = Path.Combine(AppContext.BaseDirectory, "Prompts", "ClasificarIntencion.yaml");
        if (!System.IO.File.Exists(yamlPath))
            return NotFound(new { error = "Archivo ClasificarIntencion.yaml no encontrado en Prompts/" });

        var yaml = await System.IO.File.ReadAllTextAsync(yamlPath);
        var promptConfig = KernelFunctionYaml.ToPromptTemplateConfig(yaml);
        var function = KernelFunctionFactory.CreateFromPrompt(promptConfig, new HandlebarsPromptTemplateFactory());
        var result = await _kernel.InvokeAsync(function, new KernelArguments { ["consulta"] = request.Prompt });
        return Ok(new { tecnica = "YAML-Template", respuesta = result.ToString() });
    }
}
