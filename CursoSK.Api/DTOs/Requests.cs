namespace CursoSK.Api.DTOs;

public record PromptRequest(string Prompt);
public record PromptConSettingsRequest(string Prompt, int MaxTokens = 200, double Temperature = 0.7);
public record ImagenRequest(string Descripcion, string? Quality = "hd", string? Style = "vivid");
public record TTSRequest(string Texto, string? Voice = "alloy");
public record BlogRequest(string Tema);
public record BlogPublicarRequest(string Tema, string WpUrl, string WpUser, string WpAppPassword);
public record ChatMensajeRequest(string Mensaje);
public record ChatImagenRequest(string ImagenUrl, string? Pregunta);
public record IndexarDocumentoRequest(string Titulo, string Contenido, string Categoria, string? Fuente);
public record BusquedaSemanticaRequest(string Consulta, int Top = 3);
