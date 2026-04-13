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

    /// <summary>Chat Completion con streaming (Server-Sent Events).</summary>
    [HttpPost("stream")]
    public async Task StreamChat([FromBody] PromptRequest request)
    {
        Response.ContentType = "text/event-stream";
        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        await foreach (var chunk in chatService.GetStreamingChatMessageContentsAsync(request.Prompt))
        {
            if (!string.IsNullOrEmpty(chunk.Content))
            {
                await Response.WriteAsync($"data: {chunk.Content}\n\n");
                await Response.Body.FlushAsync();
            }
        }
        await Response.WriteAsync("data: [DONE]\n\n");
    }
}
