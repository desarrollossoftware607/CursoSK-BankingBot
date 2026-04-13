using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursoSK.BankingBot.Data;
using CursoSK.BankingBot.DTOs;
using CursoSK.BankingBot.Services;

namespace CursoSK.BankingBot.Controllers;

/// <summary>Chat con historial persistente en SQLite.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("💬 Chat Bancario")]
public class ChatController : ControllerBase
{
    private readonly ConversationService _conversationService;
    private readonly BankingDbContext _db;

    public ChatController(ConversationService conversationService, BankingDbContext db)
    {
        _conversationService = conversationService;
        _db = db;
    }

    /// <summary>Envía un mensaje al chat bancario (crea sesión si no existe).</summary>
    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
    {
        var (respuesta, session) = await _conversationService.EnviarMensaje(sessionId, request.Mensaje);
        return Ok(new { sessionId, titulo = session.Titulo, respuesta });
    }

    /// <summary>Obtiene el historial completo de una sesión.</summary>
    [HttpGet("{sessionId}/historial")]
    public async Task<IActionResult> ObtenerHistorial(string sessionId)
    {
        var session = await _db.ChatSessions
            .Include(s => s.Mensajes.OrderBy(m => m.Fecha))
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session == null) return NotFound(new { error = "Sesión no encontrada" });

        return Ok(new
        {
            session.SessionId,
            session.Titulo,
            session.Canal,
            session.FechaCreacion,
            mensajes = session.Mensajes.Select(m => new { m.Rol, m.Contenido, m.Fecha })
        });
    }

    /// <summary>Lista todas las sesiones de chat.</summary>
    [HttpGet("sesiones")]
    public async Task<IActionResult> ListarSesiones()
    {
        var sesiones = await _db.ChatSessions
            .OrderByDescending(s => s.UltimaActividad)
            .Select(s => new
            {
                s.SessionId,
                s.Titulo,
                s.Canal,
                s.FechaCreacion,
                s.UltimaActividad,
                s.Activa,
                totalMensajes = s.Mensajes.Count
            })
            .ToListAsync();
        return Ok(sesiones);
    }

    /// <summary>Elimina una sesión y todos sus mensajes.</summary>
    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> EliminarSesion(string sessionId)
    {
        var session = await _db.ChatSessions
            .Include(s => s.Mensajes)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);

        if (session == null) return NotFound();

        _db.ChatMessages.RemoveRange(session.Mensajes);
        _db.ChatSessions.Remove(session);
        await _db.SaveChangesAsync();

        return Ok(new { mensaje = "Sesión eliminada" });
    }
}
