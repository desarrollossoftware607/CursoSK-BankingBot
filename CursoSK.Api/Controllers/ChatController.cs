using Microsoft.AspNetCore.Mvc;
using CursoSK.Api.DTOs;
using CursoSK.Api.Services;

namespace CursoSK.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("4️⃣ Chat — Sesiones 3-4")]
public class ChatController : ControllerBase
{
    private readonly ChatSessionService _chatService;

    public ChatController(ChatSessionService chatService) => _chatService = chatService;

    /// <summary>Envía un mensaje de texto a una sesión (mantiene historial).</summary>
    [HttpPost("{sessionId}/mensaje")]
    public async Task<IActionResult> EnviarMensaje(string sessionId, [FromBody] ChatMensajeRequest request)
    {
        var respuesta = await _chatService.EnviarMensaje(sessionId, request.Mensaje);
        return Ok(new { sessionId, respuesta });
    }

    /// <summary>Envía una imagen (URL) al chat para análisis multimodal.</summary>
    [HttpPost("{sessionId}/imagen")]
    public async Task<IActionResult> EnviarImagen(string sessionId, [FromBody] ChatImagenRequest request)
    {
        var respuesta = await _chatService.EnviarMensajeConImagen(
            sessionId,
            request.Pregunta ?? "Describe esta imagen en detalle",
            request.ImagenUrl);
        return Ok(new { sessionId, respuesta });
    }

    /// <summary>Obtiene el historial completo de una sesión.</summary>
    [HttpGet("{sessionId}/historial")]
    public IActionResult ObtenerHistorial(string sessionId)
    {
        var history = _chatService.ObtenerHistorial(sessionId);
        if (history == null) return NotFound(new { error = "Sesión no encontrada" });
        return Ok(history.Select(m => new { rol = m.Role.Label, contenido = m.Content }));
    }

    /// <summary>Lista todas las sesiones activas.</summary>
    [HttpGet("sesiones")]
    public IActionResult ListarSesiones()
        => Ok(_chatService.ListarSesiones());

    /// <summary>Elimina una sesión de chat.</summary>
    [HttpDelete("{sessionId}")]
    public IActionResult EliminarSesion(string sessionId)
    {
        _chatService.EliminarSesion(sessionId);
        return Ok(new { mensaje = "Sesión eliminada" });
    }
}
