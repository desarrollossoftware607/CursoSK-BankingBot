using Microsoft.AspNetCore.Mvc;
using CursoSK.BankingBot.Services;

namespace CursoSK.BankingBot.Controllers;

/// <summary>Transcripción de audio con Azure OpenAI Whisper.</summary>
[ApiController]
[Route("api/[controller]")]
[Tags("🎙️ Audio / Whisper")]
public class AudioController : ControllerBase
{
    private readonly AudioTranscriptionService _audioService;

    public AudioController(AudioTranscriptionService audioService)
    {
        _audioService = audioService;
    }

    /// <summary>Transcribe un archivo de audio (mp3, wav, m4a, ogg, webm, flac) a texto.</summary>
    [HttpPost("transcribir")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Transcribir(IFormFile audio)
    {
        if (audio == null || audio.Length == 0)
            return BadRequest(new { error = "Debe enviar un archivo de audio" });

        var extensiones = new[] { ".mp3", ".wav", ".m4a", ".ogg", ".webm", ".flac", ".mp4", ".oga" };
        var ext = Path.GetExtension(audio.FileName).ToLowerInvariant();
        if (!extensiones.Contains(ext))
            return BadRequest(new { error = $"Formato no soportado: {ext}. Formatos válidos: {string.Join(", ", extensiones)}" });

        using var stream = audio.OpenReadStream();
        var resultado = await _audioService.TranscribeAsync(stream, audio.FileName);

        if (!resultado.Exito)
            return StatusCode(502, new { error = resultado.Error });

        return Ok(new
        {
            texto = resultado.Texto,
            archivo = audio.FileName,
            tamaño = $"{audio.Length / 1024.0:F1} KB"
        });
    }
}
