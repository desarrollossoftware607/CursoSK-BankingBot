using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Data;
using CursoSK.BankingBot.Models;

namespace CursoSK.BankingBot.Services;

/// <summary>Servicio de conversación con persistencia en SQLite.</summary>
public class ConversationService
{
    private readonly Kernel _kernel;
    private readonly IServiceProvider _serviceProvider;

    public ConversationService(Kernel kernel, IServiceProvider serviceProvider)
    {
        _kernel = kernel;
        _serviceProvider = serviceProvider;
    }

    public async Task<(string Respuesta, ChatSession Session)> EnviarMensaje(string sessionId, string mensaje, string? systemPrompt = null)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();

        var session = await db.ChatSessions
            .Include(s => s.Mensajes)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session == null)
        {
            session = new ChatSession
            {
                SessionId = sessionId,
                Titulo = mensaje.Length > 80 ? mensaje[..80] + "..." : mensaje
            };
            db.ChatSessions.Add(session);
            await db.SaveChangesAsync();

            db.ChatMessages.Add(new ChatMessage
            {
                ChatSessionId = session.Id,
                Rol = "system",
                Contenido = systemPrompt ?? "Eres un asistente bancario especializado en préstamos y onboarding de clientes. Guías al usuario paso a paso con información clara y precisa."
            });
        }

        db.ChatMessages.Add(new ChatMessage
        {
            ChatSessionId = session.Id,
            Rol = "user",
            Contenido = mensaje
        });
        await db.SaveChangesAsync();

        // Reconstruir historial
        var mensajes = await db.ChatMessages
            .Where(m => m.ChatSessionId == session.Id)
            .OrderBy(m => m.Fecha)
            .ToListAsync();

        var chatHistory = new ChatHistory();
        foreach (var m in mensajes)
        {
            switch (m.Rol)
            {
                case "system": chatHistory.AddSystemMessage(m.Contenido); break;
                case "user": chatHistory.AddUserMessage(m.Contenido); break;
                case "assistant": chatHistory.AddAssistantMessage(m.Contenido); break;
            }
        }

        var chatService = _kernel.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentAsync(chatHistory);

        db.ChatMessages.Add(new ChatMessage
        {
            ChatSessionId = session.Id,
            Rol = "assistant",
            Contenido = response.Content!
        });
        session.UltimaActividad = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return (response.Content!, session);
    }
}
