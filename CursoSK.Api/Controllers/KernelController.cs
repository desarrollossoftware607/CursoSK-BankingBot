using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using CursoSK.Api.DTOs;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("1️⃣ Kernel — Sesión 1")]
public class KernelController : ControllerBase
{
    private readonly Kernel _kernel;
    public KernelController(Kernel kernel) => _kernel = kernel;

    /// <summary>Genera texto a partir de un prompt libre.</summary>
    [HttpPost("prompt")]
    public async Task<IActionResult> InvokePrompt([FromBody] PromptRequest request)
    {
        var result = await _kernel.InvokePromptAsync(request.Prompt);
        return Ok(new { response = result.ToString() });
    }

    /// <summary>Genera texto con parámetros de ejecución (temperatura, max tokens).</summary>
    [HttpPost("prompt/configurado")]
    public async Task<IActionResult> InvokePromptConSettings([FromBody] PromptConSettingsRequest request)
    {
        var settings = new OpenAI.Chat.ChatCompletionOptions(); // SK wraps this
        var skSettings = new Microsoft.SemanticKernel.Connectors.OpenAI.OpenAIPromptExecutionSettings
        {
            MaxTokens = request.MaxTokens,
            Temperature = request.Temperature
        };
        var result = await _kernel.InvokePromptAsync(request.Prompt, new KernelArguments(skSettings));
        return Ok(new { response = result.ToString(), maxTokens = request.MaxTokens, temperature = request.Temperature });
    }
}
