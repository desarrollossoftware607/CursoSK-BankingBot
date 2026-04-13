using System.Collections.Concurrent;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace CursoSK.Api.Services;

public class ChatSessionService
{
    private readonly ConcurrentDictionary<string, ChatHistory> _sessions = new();
    private readonly Kernel _kernel;

    public ChatSessionService(Kernel kernel) => _kernel = kernel;

    public async Task<string> EnviarMensaje(string sessionId, string mensaje)
    {
        var history = _sessions.GetOrAdd(sessionId, _ =>
            new ChatHistory("Eres un asistente útil y amable que recuerda toda la conversación."));

        history.AddUserMessage(mensaje);
        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(history);
        history.AddAssistantMessage(response.Content!);

        return response.Content!;
    }

    public async Task<string> EnviarMensajeConImagen(string sessionId, string pregunta, string imagenUrl)
    {
        var history = _sessions.GetOrAdd(sessionId, _ =>
            new ChatHistory("Eres un asistente multimodal que puede analizar imágenes."));

        var contents = new ChatMessageContentItemCollection
        {
            new TextContent(pregunta),
            new ImageContent(new Uri(imagenUrl))
        };
        history.AddUserMessage(contents);

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(history);
        history.AddAssistantMessage(response.Content!);
        return response.Content!;
    }

    public ChatHistory? ObtenerHistorial(string sessionId)
        => _sessions.GetValueOrDefault(sessionId);

    public bool EliminarSesion(string sessionId)
        => _sessions.TryRemove(sessionId, out _);

    public IEnumerable<string> ListarSesiones()
        => _sessions.Keys;
}
